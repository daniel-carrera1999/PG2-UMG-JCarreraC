using AdoptaYA.Functionalities.Profile.Http;
using AdoptaYA.Functionalities.Profile.Model;
using Microsoft.AspNetCore.Components;

namespace AdoptaYA.Functionalities.Profile.Components;
public partial class UpdatePersonalReferences
{
    [Parameter] public EventCallback OnReloadData { get; set; }
    [Parameter] public ApplicantResponseDTO applicant { get; set; }

    [Inject] ProfileHttp ProfileHttp { get; set; } = default!;

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
        loading = true;
        await ProfileHttp.UpdatePersonalReferences(applicant.Id, references);
        await OnReloadData.InvokeAsync();
        loading = false;

        StateHasChanged();
    }

    private async void Cancel()
    {
        await LoadDataAsync();
        await OnReloadData.InvokeAsync();
    }

}