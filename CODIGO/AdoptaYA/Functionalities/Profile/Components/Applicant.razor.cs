using AdoptaYA.Functionalities.Profile.Http;
using AdoptaYA.Functionalities.Profile.Model;
using AdoptaYA.Services.Security;
using Microsoft.AspNetCore.Components;

namespace AdoptaYA.Functionalities.Profile.Components;
public partial class Applicant
{
    [Parameter] public EventCallback OnReloadDataProfile { get; set; }
    [Inject] GetCurrentUser GetCurrentUser { get; set; } = default!;
    [Inject] ProfileHttp ProfileHttp { get; set; } = default!;

    private bool loading = false;
    private bool ShowCreateApplicant = false;
    private bool ShowUpdateApplicant = false;

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

    private async void ReloadDataAsync()
    {
        loading = true;
        ShowCreateApplicant = false;
        ShowUpdateApplicant = false;
        await LoadDataAsync();
        await OnReloadDataProfile.InvokeAsync();
        loading = false;

        StateHasChanged();
    }
}