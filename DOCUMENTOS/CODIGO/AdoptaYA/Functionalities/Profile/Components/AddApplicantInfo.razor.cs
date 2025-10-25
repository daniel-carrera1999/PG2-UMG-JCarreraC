using AdoptaYA.Functionalities.Profile.Http;
using AdoptaYA.Functionalities.Profile.Model;
using AdoptaYA.Services.Security;
using Microsoft.AspNetCore.Components;

namespace AdoptaYA.Functionalities.Profile.Components;
public partial class AddApplicantInfo
{
    [Parameter] public EventCallback OnReloadData { get; set; }

    [Inject] GetCurrentUser GetCurrentUser { get; set; } = default!;
    [Inject] ProfileHttp ProfileHttp { get; set; } = default!;

    private ApplicantRequestDTO applicantDTO = new();
    private List<string> MaritalStatusList = new List<string>();

    bool loading = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
        MaritalStatusList.Add("Soltero (a)");
        MaritalStatusList.Add("Casado (a)");
        MaritalStatusList.Add("Divorciado (a)");
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        var user = await GetCurrentUser.GetUserInfoAsync();
        applicantDTO.Email = user.EmailAddress;
        applicantDTO.IdUsuario = user.UserId!.Value;
        loading = false;

        StateHasChanged();
    }

    private async Task SubmitAsync()
    {
        loading = true;
        try
        {
            await ProfileHttp.CreateApplicant(applicantDTO);
            await OnReloadData.InvokeAsync();

        }
        finally
        {
            loading = false;
        }
    }

    private async Task Cancel()
    {
        var user = await GetCurrentUser.GetUserInfoAsync();
        applicantDTO = new();
        applicantDTO.Email = user.EmailAddress;
        applicantDTO.IdUsuario = user.UserId!.Value;
        await OnReloadData.InvokeAsync();
    }

}