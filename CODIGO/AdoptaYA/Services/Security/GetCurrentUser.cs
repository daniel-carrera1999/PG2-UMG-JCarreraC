using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
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

    public async Task<AuthUserInfoResult> GetUserInfoAsync()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            return new AuthUserInfoResult(null, null, null, null, null, null, null, null);
        }

        var jwtToken = user.FindFirstValue("AccessToken");
        var handler = new JwtSecurityTokenHandler();
        var accessToken = handler.ReadJwtToken(jwtToken);
        var userIdClaim = accessToken.Claims.FirstOrDefault(c => c.Type == "UserId");

        int _userId = 0;
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var parsedId))
        {
            _userId = parsedId;
        }

        int _employeeCode = int.TryParse(user.FindFirstValue("EmployeeCode"), out int code) ? code : 0;
        string _name = user.FindFirstValue("name")!;
        string _emailAddress = user.FindFirstValue("emailaddress")!;
        string _role = user.FindFirstValue("role")!;
        int _locationId = int.TryParse(user.FindFirstValue("LocationId"), out int locId) ? locId : 0;
        int _storeId = int.TryParse(user.FindFirstValue("StoreId"), out int storeId) ? storeId : 0;
        string _store = user.FindFirstValue("store")!;

        return new AuthUserInfoResult(_employeeCode, _userId, _name, _emailAddress, _role, _locationId, _storeId, _store);

    }

}
