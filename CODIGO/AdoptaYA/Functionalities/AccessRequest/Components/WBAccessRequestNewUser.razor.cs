using Microsoft.AspNetCore.Components;
using AdoptaYA.Services.Http;
using Radzen;
using AdoptaYA.Functionalities.AccessRequest.Model;
using AdoptaYA.Services.Dialogs;

namespace AdoptaYA.Functionalities.AccessRequest.Components;
public partial class WBAccessRequestNewUser
{
    [Inject] private ApiPostService ApiPostService { get; set; } = default!;
    [Inject] private DialogService DialogService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private CustomDialogService CustomDialogService { get; set; } = default!;

    bool readOnlyCodigo = true;

    public CreateNewUserForm Form = new();
    bool busy;

    private async Task OnSubmitAsync()
    {
        busy = true;

        int StatusCode = 0;

        try
        {
            var response = await ApiPostService.PostAsync("usuario", Form, 2, false);

            if (response.IsSuccessStatusCode)
            {
                await DialogService.Alert("Usuario creado con éxito.", "Operación exitosa", new AlertOptions() { OkButtonText = "Aceptar", CloseDialogOnEsc = false, CloseDialogOnOverlayClick=false, ShowClose=false });
                StatusCode = (int)response.StatusCode;
            }
            else
            {
                await CustomDialogService.OpenViewErrorsAsync(response);
                StatusCode = (int)response.StatusCode;
            }
        }
        catch (Exception e)
        {
            await DialogService.Alert(e.Message, "Error de conexión", new AlertOptions() { OkButtonText = "Aceptar" });
        }
        finally
        {
            if (StatusCode != 401)
            {
                ToLogin();
            }
            busy = false;
        }
    }

    public void ToLogin()
    {
        NavigationManager.NavigateTo("/");
    }
}