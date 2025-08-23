using Microsoft.AspNetCore.Components;
using AdoptaYA.Services.Http;
using Radzen;
using AdoptaYA.Functionalities.RecoveryPassword.Model;
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Extensions;

namespace AdoptaYA.Functionalities.RecoveryPassword.Pages;
public partial class ResetPasswordRequest
{
    [Inject] ApiPostService ApiPostService { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    [Inject] CustomDialogService CustomDialogService { get; set; } = default!;

    public ResetPasswordRequestForm Request = new();
    public bool busy;

    public async Task OnSubmitAsync()
    {
        try
        {
            busy = true;

            var response = await ApiPostService.PostAsync($"auth/recuperar-credenciales?employeeCode={Request.EmployeeCode}", Request, 2, false);

            if (response.IsSuccessStatusCode)
            {
                var close = await DialogService.Alert("Solicitud realizada con éxito", "Operación éxitosa", new AlertOptions { OkButtonText = "Aceptar", CloseDialogOnEsc = false, ShowClose = false, CloseDialogOnOverlayClick = false });
                if (close == true)
                {
                    NavigationManager.NavigateTo("/", forceLoad: true);
                }
            }
            else
            {
                await CustomDialogService.OpenViewErrorsAsync(response);
            }
        }
        catch (Exception e)
        {
            await DialogService.Alert(e.Message, "Error Inesperado", new AlertOptions() { OkButtonText = "Aceptar" });
        }
        finally
        {
            busy = false;
        }
    }

}