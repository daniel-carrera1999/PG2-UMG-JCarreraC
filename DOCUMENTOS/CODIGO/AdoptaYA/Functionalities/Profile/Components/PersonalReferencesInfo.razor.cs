using AdoptaYA.Functionalities.Profile.Http;
using AdoptaYA.Functionalities.Profile.Model;
using AdoptaYA.Services.Http;
using Microsoft.AspNetCore.Components;

namespace AdoptaYA.Functionalities.Profile.Components;
public partial class PersonalReferencesInfo
{
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

}