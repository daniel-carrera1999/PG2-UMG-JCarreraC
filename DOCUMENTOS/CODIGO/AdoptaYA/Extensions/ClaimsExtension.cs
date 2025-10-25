using AdoptaYA.Functionalities.Authentication.Model.Session;
using System.Security.Claims;
using System.Text.Json;

namespace AdoptaYA.Extensions;

public static class ClaimsExtensions
{
    public static SessionUser ReconstructUserFromClaims(this ClaimsPrincipal user)
    {
        var usuario = new SessionUser
        {
            Username = user.FindFirst("Username")?.Value ?? string.Empty,
            Name = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
            Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
            Rol = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
            Menu = user.Claims
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
                .ToList()!
        };

        return usuario;
    }
}
