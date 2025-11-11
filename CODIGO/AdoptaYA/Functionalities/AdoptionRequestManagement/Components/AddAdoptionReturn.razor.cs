using AdoptaYA.Functionalities.AdoptionRequestManagement.Http;
using AdoptaYA.Functionalities.AdoptionRequestManagement.Model;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.Components;

public partial class AddAdoptionReturn
{
    [Inject] AdoptionRequestManagementHttp http { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;
    [Inject] NotificationService NotificationService { get; set; } = default!;

    [Parameter] public int AdoptionId { get; set; }
    [Parameter] public int UserId { get; set; }

    private bool loading = false;
    private AdoptionRequestReturnDTO model = new();

    protected override void OnInitialized()
    {
        model = new AdoptionRequestReturnDTO
        {
            AdoptionId = AdoptionId,
            UserId = UserId
        };
    }

    private async void OnSubmit(AdoptionRequestReturnDTO args)
    {
        loading = true;
        bool res = await http.AddAdoptionReturn(model);
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