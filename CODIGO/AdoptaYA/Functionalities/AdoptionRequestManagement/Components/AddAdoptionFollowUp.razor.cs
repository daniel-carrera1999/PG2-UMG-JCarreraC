using AdoptaYA.Functionalities.AdoptionRequestManagement.Http;
using AdoptaYA.Functionalities.AdoptionRequestManagement.Model;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.Components;

public partial class AddAdoptionFollowUp
{
    [Inject] AdoptionRequestManagementHttp http { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;
    [Inject] NotificationService NotificationService { get; set; } = default!;

    [Parameter] public EventCallback OnSaveFollowUp { get; set; }
    [Parameter] public int AdoptionId { get; set; }
    [Parameter] public int UserId { get; set; }

    private bool loading = false;

    private AdoptionRequestFollowUpDTO model = new();

    protected override void OnInitialized()
    {
        model = new AdoptionRequestFollowUpDTO
        {
            AdoptionId = AdoptionId,
            UserId = UserId
        };
    }

    private async void OnSubmit(AdoptionRequestFollowUpDTO args)
    {
        loading = true;
        bool res = await http.AddAdopionFollowUp(model);
        loading = false;

        if (res == true)
        {
            NotificationService.Notify(NotificationSeverity.Success, "Registro exitoso", "El seguimiento ha sido ingresado correctamente.", 4000);
            DialogService.Close(true);
        }
    }

    private void OnCancel()
    {
        DialogService.Close(false);
    }
}