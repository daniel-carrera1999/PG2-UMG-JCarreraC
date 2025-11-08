using AdoptaYA.Functionalities.Profile.Http;
using AdoptaYA.Functionalities.Profile.Model;
using Microsoft.AspNetCore.Components;

namespace AdoptaYA.Functionalities.Profile.Components;
public partial class UpdateApplicationInfo
{
    [Parameter] public EventCallback OnReloadData { get; set; }
    [Parameter] public ApplicantResponseDTO applicant { get; set; }

    [Inject] ProfileHttp ProfileHttp { get; set; } = default!;

    private ApplicantUpdateRequestDTO applicationDTO = new();
    private List<string> MaritalStatusList = new()
    {
        "Soltero(a)",
        "Casado(a)",
        "Divorciado(a)"
    };

    private bool loading = false;

    protected override async void OnInitialized()
    {
        LoadData();
    }

    private void LoadData()
    {
        applicationDTO = new ApplicantUpdateRequestDTO
        {
            Id = applicant.Id,
            IdUser = applicant.IdUser,
            Inactive = applicant.Inactive,
            Name = applicant.Name,
            LastName = applicant.LastName,
            Birthdate = applicant.Birthdate,
            CellPhone = applicant.CellNumber,
            HomePhone = applicant.HomePhone,
            Email = applicant.Email,
            Address = applicant.Address,
            MonthlyIncome = applicant.MonthlyIncome,
            MaritalStatus = applicant.MaritalStatus,
            Occupation = applicant.Occupation,
            Date = applicant.Date
        };
    }

    private async Task SubmitEditAsync()
    {
        loading = true;
        await ProfileHttp.UpdateApplicant(applicationDTO.Id, applicationDTO);
        loading = false;

        await OnReloadData.InvokeAsync();
        StateHasChanged();
    }

    private async void CancelEdit()
    {
        await OnReloadData.InvokeAsync();
    }
}