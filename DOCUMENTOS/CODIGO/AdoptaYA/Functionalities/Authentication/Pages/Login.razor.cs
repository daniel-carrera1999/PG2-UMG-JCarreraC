using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System.Security.Claims;
using System.Text.Json;
using AdoptaYA.Services.Http;
using AdoptaYA.Shared.Components;
using Microsoft.AspNetCore.Components.Authorization;
using AdoptaYA.Functionalities.Authentication.Model.Form;
using AdoptaYA.Functionalities.Authentication.Model.Session;
using AdoptaYA.Shared.Model;
using AdoptaYA.Shared.Interfaces;
using System;
using AdoptaYA.Services.Dialogs;

namespace AdoptaYA.Functionalities.Authentication.Pages;
public partial class Login
{
    [Inject] private ApiPostService ApiPostService { get; set; } = default!;
    [Inject] private DialogService DialogService { get; set; } = default!;
    [Inject] private CustomDialogService CustomDialogService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
    [Inject] private IAuthCookieService AuthCookieService { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;

    public string subtitle = "Por favor ingresa tus credenciales para continuar.";
    public bool requiredAuthenticatorCode = false;
    private LoginForm formLogin = new();
    bool busy;

    private async Task OnSubmitAsync()
    {
        busy = true;

        try
        {
            var response = await ApiPostService.PostAsync("auth/login", formLogin, 2, true);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                await ProcessSuccessfulLoginAsync(response, content);
            }
            else
            {
                await CustomDialogService.OpenViewErrorsAsync(response);
            }
        }
        catch (Exception e)
        {
            await CustomDialogService.OpenInternalErrorAsync(e);
        }
        finally
        {
            busy = false;
        }
    }

    private async Task ProcessSuccessfulLoginAsync(HttpResponseMessage response, string content)
    {
        var token = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
        var user = JsonSerializer.Deserialize<SessionUser>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var cookieRefreshToken = await AuthCookieService.GetAuthCookie(response);

        if (string.IsNullOrEmpty(token) || user == null)
        {
            await DialogService.Alert("No se pudo procesar el inicio de sesión", "Error de autenticación", new AlertOptions { OkButtonText = "Aceptar" });
            return;
        }

        var userJson = JsonSerializer.Serialize(user);

        //await JS.InvokeVoidAsync("auth.setTempSession", token, userJson);
        await JS.InvokeVoidAsync("auth.setTempSession", token, "");
        await JS.InvokeVoidAsync("auth.setTempUserFragmented", userJson, 2000);

        await JS.InvokeVoidAsync("auth.setTempRefreshToken", cookieRefreshToken!.RefreshToken, cookieRefreshToken.Expires?.ToString("o"));
        NavigationManager.NavigateTo("/auth/start-session", forceLoad: true);
    }


    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        string _exp = user.FindFirstValue("exp") ?? "";

        if (DateTime.TryParse(_exp, out var expDateTime))
        {
            if (user.Identity?.IsAuthenticated == true || expDateTime >= DateTime.UtcNow)
            {
                NavigationManager.NavigateTo("/home", forceLoad: true);
            }
        }
    }

}