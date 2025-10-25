using AdoptaYA.Functionalities.Authentication.Model.Session;
using AdoptaYA.Services.Security;
using AdoptaYA.Services.SGL;
using AdoptaYA.Services.UI;
using AdoptaYA.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace AdoptaYA.Components.Layout;
public partial class NavMenu
{
    [Inject] MainLayoutService LayoutService { get; set; } = default!;
    [Inject] GetCurrentUser GetCurrentUser { get; set; } = default!;

    private bool sidebarExpanded;
    public string name { get; set; } = "Usuario";
    public string rol { get; set; } = "Usuario";

    private List<Menu> menus = new();

    protected override void OnInitialized()
    {
        sidebarExpanded = LayoutService.GetSidebarExpanded();
        LayoutService.OnSidebarExpandedChanged += OnSidebarStateChanged;
        LayoutService.OnSidebarUserNameChanged += async (nuevoTexto) =>
        {
            name = nuevoTexto;
            await InvokeAsync(StateHasChanged);
        };
        LayoutService.OnSidebarRolChanged += async (nuevoTexto) =>
        {
            rol = nuevoTexto;
            await InvokeAsync(StateHasChanged);
        };
    }

    private void OnSidebarStateChanged(bool expanded)
    {
        sidebarExpanded = expanded;
        InvokeAsync(StateHasChanged);
    }

    protected override async Task OnInitializedAsync()
    {
        menus = await GetCurrentUser.GetMenusAsync();
    }
}