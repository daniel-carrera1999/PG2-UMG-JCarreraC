using AdoptaYA.Functionalities.Profile.Http;
using AdoptaYA.Functionalities.Profile.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace AdoptaYA.Functionalities.Profile.Components;
public partial class UpdateApplicationInfo
{
    [Parameter] public EventCallback OnReloadData { get; set; }
    [Parameter] public ApplicantResponseDTO applicant { get; set; }

    [Inject] ProfileHttp ProfileHttp { get; set; } = default!;
    [Inject] NotificationService NotificationService { get; set; } = default!;

    private ApplicantUpdateRequestDTO applicationDTO = new();
    private List<string> MaritalStatusList = new()
    {
        "Soltero (a)",
        "Casado (a)",
        "Divorciado (a)",
        "Viudo (a)"
    };

    private bool loading = false;

    protected override async void OnInitialized()
    {
        LoadDataAsync();
    }

    private void LoadDataAsync()
    {
        applicationDTO = new ApplicantUpdateRequestDTO
        {
            Id = applicant.Id,
            IdUser = applicant.IdUser,
            Inactive = applicant.Inactive,
            Name = applicant.Name,
            LastName = applicant.LastName,
            Dpi = applicant.Dpi,
            Birthdate = applicant.Birthdate,
            CellPhone = applicant.CellNumber,
            HomePhone = applicant.HomePhone,
            Email = applicant.Email,
            Address = applicant.Address,
            MonthlyIncome = applicant.MonthlyIncome,
            MaritalStatus = applicant.MaritalStatus,
            Occupation = applicant.Occupation,
            RequestPhoto = applicant.RequestPhoto,
            Date = applicant.Date
        };
    }
    private async Task OnFileChange(string e)
    {
        // Radzen pasa un 'object' que contiene la propiedad Files
        dynamic eventArgs = e;
        if (eventArgs?.Files == null || eventArgs.Files.Count == 0)
            return;

        var file = (IBrowserFile)eventArgs.Files[0];
        const long maxFileSize = 10 * 1024 * 1024; // 10MB

        if (file.Size > maxFileSize)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Archivo muy grande",
                Detail = "La imagen no puede pesar más de 10 MB",
                Duration = 4000
            });
            return;
        }

        try
        {
            using var memoryStream = new MemoryStream();
            await file.OpenReadStream(maxAllowedSize: maxFileSize).CopyToAsync(memoryStream);
            var base64 = Convert.ToBase64String(memoryStream.ToArray());
            var dataUrl = $"data:{file.ContentType};base64,{base64}";

            applicationDTO.RequestPhoto = dataUrl;

            StateHasChanged();
        }
        catch (Exception ex)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Error",
                Detail = $"No se pudo cargar la imagen: {ex.Message}",
                Duration = 4000
            });
        }
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