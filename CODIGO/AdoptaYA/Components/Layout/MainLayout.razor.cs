using AdoptaYA.Services.Security;
using AdoptaYA.Services.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace AdoptaYA.Components.Layout;
public partial class MainLayout
{
    [Inject] MainLayoutService LayoutService { get; set; } = default!;
    [Inject] HttpClient HttpClient { get; set; } = default!;
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;
    [Inject] CustomAuthenticationStateProvider _authStateProvider { get; set; } = default!;
    [Inject] AuthSessionService AuthSessionService { get; set; } = default!;
    [Inject] IJSRuntime JSRuntime { get; set; } = default!;

    public string title { get; set; } = "Default";
    public string name { get; set; } = "Usuario";
    public string email { get; set; } = "------";

    private bool _jsAvailable = false;

    protected override void OnInitialized()
    {
        LayoutService.OnHeaderTextChanged += async (nuevoTexto) =>
        {
            title = nuevoTexto;
            await InvokeAsync(StateHasChanged);
        };

        LayoutService.OnHeaderUserNameChanged += async (nuevoTexto) =>
        {
            name = nuevoTexto;
            await InvokeAsync(StateHasChanged);
        };

        LayoutService.OnHeaderEmailChanged += async (nuevoValor) =>
        {
            email = nuevoValor;
            await InvokeAsync(StateHasChanged);
        };
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            _jsAvailable = true;
        }
    }

    void ToggleSidebar()
    {
        var newState = !LayoutService.GetSidebarExpanded();
        LayoutService.SetSidebarExpanded(newState);
    }

    private async Task LogoutAsync()
    {
        if (_jsAvailable)
        {
            await JSRuntime.InvokeVoidAsync("localStorage.setItem", "SessionReason", "manual");
            await Task.Delay(200);
        }

        AuthSessionService.CloseSession();
    }
}