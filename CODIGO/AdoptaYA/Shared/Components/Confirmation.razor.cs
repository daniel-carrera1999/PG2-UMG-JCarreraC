using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdoptaYA.Shared.Components;
public partial class Confirmation
{
    [Inject] DialogService DialogService { get; set; } = default!;

    [Parameter] public string? Message { get; set; }
    [Parameter] public string ConfirmText { get; set; } = "Confirmar";
    [Parameter] public string CancelText { get; set; } = "Cancelar";


    private void Confirm()
    {
        DialogService.Close(true);
    }

    private void Cancel()
    {
        DialogService.Close(false);
    }
}