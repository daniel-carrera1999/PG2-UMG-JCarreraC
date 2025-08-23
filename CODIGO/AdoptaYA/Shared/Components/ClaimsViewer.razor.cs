using System.Security.Claims;
using System.Text.Json;
using AdoptaYA.Functionalities.Authentication.Model.Session;
using Radzen;

namespace AdoptaYA.Shared.Components;
public partial class ClaimsViewer
{
    private ClaimsPrincipal? user;
    private List<Menu> menus = new();

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            menus = user.Claims
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
        }
    }
}