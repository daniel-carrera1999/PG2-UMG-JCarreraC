using AdoptaYA.Functionalities.Profile.Http;
using AdoptaYA.Functionalities.Profile.Model;
using AdoptaYA.Services.Dialogs;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdoptaYA.Functionalities.Profile.Components;
public partial class UpdatePersonalReferences
{
    [Parameter] public EventCallback OnReloadData { get; set; }
    [Parameter] public ApplicantResponseDTO applicant { get; set; }

    [Inject] ProfileHttp ProfileHttp { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;

    private bool loading = false;

    private IList<PersonalReferenceResponseDTO> references = new List<PersonalReferenceResponseDTO>();

    protected override async void OnInitialized()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        references = await ProfileHttp.GetPersonalReferencesByApplicantId(applicant.Id);
        loading = false;

        StateHasChanged();
    }

    private void AddReference()
    {
        references ??= new List<PersonalReferenceResponseDTO>();

        references.Add(new PersonalReferenceResponseDTO
        {
            Id = 0,
            IdApplicant = applicant.Id,
            Name = string.Empty,
            CellPhone = string.Empty,
            Relationship = string.Empty,
            Date = DateTime.Now,
            Inactive = 0
        });

        StateHasChanged();
    }

    private void RemoveReference(int index)
    {
        if (references != null && index >= 0 && index < references.Count)
        {
            references.RemoveAt(index);
            StateHasChanged();
        }
    }

    private async void Save()
    {
        string validationMessage = GetReferencesValidationMessage();

        if (!string.IsNullOrEmpty(validationMessage))
        {
            await DialogService.Alert(validationMessage, "Datos de Referencias Incompletos", new AlertOptions { CloseDialogOnEsc=false, CloseDialogOnOverlayClick=false, ShowClose=false, OkButtonText="Aceptar" });
        }
        else
        {
            loading = true;
            await ProfileHttp.UpdatePersonalReferences(applicant.Id, references);
            await OnReloadData.InvokeAsync();
            loading = false;

            StateHasChanged();
        }
    }

    private async void Cancel()
    {
        await LoadDataAsync();
        await OnReloadData.InvokeAsync();
    }

    private string GetReferencesValidationMessage()
    {
        if (!references.Any())
        {
            return "Debe agregar al menos una referencia personal.";
        }

        var incompleteReferences = references
            .Where(r => string.IsNullOrWhiteSpace(r.Name) ||
                        string.IsNullOrWhiteSpace(r.CellPhone) ||
                        string.IsNullOrWhiteSpace(r.Relationship))
            .Count();

        if (incompleteReferences > 0)
        {
            if (incompleteReferences == 1)
            {
                return "Hay una referencia incompleta. Por favor, complete todos los campos (nombre, teléfono y relación).";
            }
            else
            {
                return $"Hay {incompleteReferences} referencias incompletas. Por favor, complete todos los campos (nombre, teléfono y relación) en cada referencia.";
            }
        }

        return string.Empty;
    }

}