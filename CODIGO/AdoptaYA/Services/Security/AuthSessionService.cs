using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using AdoptaYA.Functionalities.Authentication.Model.Session;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdoptaYA.Services.Security;

public class AuthSessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly NavigationManager _navigationManager;

    public AuthSessionService(IHttpContextAccessor httpContextAccessor, 
                              NavigationManager navigationManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _navigationManager = navigationManager;
    }

    public async Task CreateSessionAsync(string tokenJwt, SessionUser user, string refreshToken, string expRefreshToken)
    {
        // Leer y validar expiración del token JWT
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(tokenJwt);
        var expUtc = DateTime.SpecifyKind(jwt.ValidTo, DateTimeKind.Utc);

        if (expUtc <= DateTime.UtcNow)
        {
            throw new SecurityTokenExpiredException("El token ya ha expirado.");
        }

        // Crear claims con la información del usuario devuelta por el API
        //var expUtcClaim = DateTime.SpecifyKind(jwt.ValidTo, DateTimeKind.Utc);
        var claims = new List<Claim>
        {
            new Claim("Username", user.Username),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Rol),
            new Claim("AccessToken", tokenJwt),
            new Claim("exp", expUtc.ToString("o")),
            new Claim("RefreshToken", refreshToken),
            new Claim("expRefreshToken", expRefreshToken),
        };

        // Crear claims individuales para las rutas y permisos
        foreach (var menu in user.Menu!)
        {
            var json = JsonSerializer.Serialize(menu);
            claims.Add(new Claim($"Menu:{menu.Path}", json));
        }

        // Crear identidad y principal
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // Crear sesión autenticada con cookie
        await _httpContextAccessor.HttpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.Parse(expRefreshToken)
            });
    }

    public void CloseSession()
    {
        _navigationManager.NavigateTo("/auth/logout", forceLoad: true);
    }

}