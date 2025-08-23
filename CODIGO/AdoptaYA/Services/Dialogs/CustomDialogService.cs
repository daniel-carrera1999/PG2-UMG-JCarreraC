using AdoptaYA.Shared.Components;
using Radzen;
using AdoptaYA.Shared.Model;
using System.Text.Json;

namespace AdoptaYA.Services.Dialogs;
public class CustomDialogService
{
    private readonly DialogService DialogService;

    public CustomDialogService(DialogService dialogService)
    {
        DialogService = dialogService;
    }

    /*public async Task<bool> OpenResolveIncidentAsync(int IncidentId, int? IncidentTypeId)
    {
        var response = await DialogService.OpenAsync<ResolveIncident>(
            $"Resolver incidencia: <strong>{IncidentId}</strong>",
            new Dictionary<string, object?>
            {
                { "IncidentId", IncidentId },
                { "IncidentTypeId", IncidentTypeId }
            },
            new DialogOptions { Width = "60%", ShowClose=false, CloseDialogOnEsc=false, CloseDialogOnOverlayClick=false }
        );
        return response;
    }*/

    public async Task<bool> OpenConfirmAsync (string Message, string Title, string ConfirmText, string CancelText, DialogOptions Options)
    {
        bool response = await DialogService.OpenAsync<Confirmation>(
            Title,
            new Dictionary<string, object?>
            {
                { "message", Message },
                { "ConfirmText", ConfirmText },
                { "CancelText", CancelText }
            },
            new DialogOptions
            {
                CloseDialogOnEsc = false,
                ShowClose = false,
                Width = Options.Width
            }
        );

        return response;
    }

    public async Task OpenViewErrorsAsync (HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(content))
        {
            await DialogService.Alert(response.ReasonPhrase, $"Error código ({(int)response.StatusCode})", new AlertOptions() { OkButtonText = "Aceptar" });
        } 
        else
        {
            var errorObject = JsonSerializer.Deserialize<ApiResponseNotAcceptable>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            await DialogService.OpenAsync<ViewErrorsModal>(
                errorObject!.Title,
                new Dictionary<string, object>
                    { { "responseNotAcceptable", errorObject } },
                new DialogOptions
                    { Width = "400px", CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false, ShowClose = false }
            );
        }
    }

    public async Task OpenInternalErrorAsync(Exception e)
    {
        await DialogService.Alert(e.Message, "Error interno", new AlertOptions { OkButtonText = "Aceptar", CloseDialogOnEsc=false, CloseDialogOnOverlayClick=false, ShowClose=false }); 
    }

}