using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using AdoptaYA.Components.Pages;
using AdoptaYA.Functionalities.Authentication.Model.Session;
using AdoptaYA.Shared.Model;
using AdoptaYA.Shared.Record;
using Microsoft.AspNetCore.Components.Authorization;

namespace AdoptaYA.Services.Security;
public class GetCurrentUser
{
    private readonly AuthenticationStateProvider _authProvider;

    public GetCurrentUser(AuthenticationStateProvider authProvider)
    {
        _authProvider = authProvider;
    }

    public async Task<List<Menu>> GetMenusAsync()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            return new List<Menu>();
        }

        var menus = user.Claims
            .Where(c => c.Type.StartsWith("Menu:"))
            .Select(c =>
            {
                try
                {
                    return JsonSerializer.Deserialize<Menu>(c.Value);
                }
                catch
                {
                    return null;
                }
            })
            .Where(m => m != null)
            .ToList()!;

        return menus!;
    }

    public async Task<Menu> GetMenuAsync(string path)
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            return new Menu();
        }

        var menu = user.Claims
            .Where(c => c.Type.StartsWith("Menu:"))
            .Select(c =>
            {
                try
                {
                    return JsonSerializer.Deserialize<Menu>(c.Value);
                }
                catch
                {
                    return null;
                }
            })
            .Where(m => m != null && m.Path == path)
            .FirstOrDefault();

        return menu! ?? new Menu();
    }

    public async Task<AuthTokenResult> GetTokenInfoAsync()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            return new AuthTokenResult(null, null, null, null);
        }

        string _accessToken = user.FindFirstValue("AccessToken") ?? "";
        string _exp = user.FindFirstValue("exp") ?? "";

        string _refreshToken = user.FindFirstValue("RefreshToken") ?? "";
        string _expRefreshToken = user.FindFirstValue("expRefreshToken") ?? "";

        return new AuthTokenResult(_accessToken, _exp, _refreshToken, _expRefreshToken);
    }

    public async Task<AuthUserInfo> GetUserInfoAsync()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            return new AuthUserInfo(null, null, null, null, null);
        }

        var jwtToken = user.FindFirstValue("AccessToken");
        var handler = new JwtSecurityTokenHandler();
        var accessToken = handler.ReadJwtToken(jwtToken);

        var userIdClaim = accessToken.Claims.FirstOrDefault(c => c.Type == "UserId");

        int _userId = 0;

        // intenta obtener el campo directamente del payload
        if (accessToken.Payload.TryGetValue("UserId", out var userIdValue) ||
            accessToken.Payload.TryGetValue("sub", out userIdValue) ||
            accessToken.Payload.TryGetValue("uid", out userIdValue))
        {
            if (int.TryParse(userIdValue?.ToString(), out var parsedId))
            {
                _userId = parsedId;
            }
        }

        string _username = user.FindFirstValue("Username")!;
        string _name = user.FindFirstValue(ClaimTypes.Name)!;
        string _emailAddress = user.FindFirstValue(ClaimTypes.Email)!;
        string _role = user.FindFirstValue(ClaimTypes.Role)!;

        return new AuthUserInfo(_userId, _username, _name, _emailAddress, _role);

    }

}
