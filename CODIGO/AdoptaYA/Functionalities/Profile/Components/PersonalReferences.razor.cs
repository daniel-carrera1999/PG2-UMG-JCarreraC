using AdoptaYA.Functionalities.Profile.Http;
using AdoptaYA.Functionalities.Profile.Model;
using AdoptaYA.Services.Security;
using Microsoft.AspNetCore.Components;

namespace AdoptaYA.Functionalities.Profile.Components;
public partial class PersonalReferences
{
    [Inject] GetCurrentUser GetCurrentUser { get; set; } = default!;
    [Inject] ProfileHttp ProfileHttp { get; set; } = default!;

    private bool loading = false;
    private bool ShowUpdateReference = false;

    private ApplicantResponseDTO applicant = new();

    protected override async Task OnInitializedAsync()
    {
        loading = true;
        await LoadDataAsync();
        loading = false;

        StateHasChanged();
    }

    private async Task LoadDataAsync()
    {
        var user = await GetCurrentUser.GetUserInfoAsync();
        applicant = await ProfileHttp.GetApplicantDetailAsync(user.UserId!.Value);
    }

    public async Task ReloadDataAsync()
    {
        await LoadDataAsync();
        ShowUpdateReference = false;
        StateHasChanged();
    }

}