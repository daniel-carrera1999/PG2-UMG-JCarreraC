using AdoptaYA.Functionalities.Profile.Http;
using AdoptaYA.Functionalities.Profile.Model;
using AdoptaYA.Services.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace AdoptaYA.Functionalities.Profile.Components;
public partial class AddApplicantInfo
{
    [Parameter] public EventCallback OnReloadData { get; set; }

    [Inject] GetCurrentUser GetCurrentUser { get; set; } = default!;
    [Inject] ProfileHttp ProfileHttp { get; set; } = default!;
    [Inject] NotificationService NotificationService { get; set; } = default!;

    private ApplicantRequestDTO applicantDTO = new();
    private List<string> MaritalStatusList = new List<string>();

    bool loading = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
        MaritalStatusList.Add("Soltero (a)");
        MaritalStatusList.Add("Casado (a)");
        MaritalStatusList.Add("Divorciado (a)");
        MaritalStatusList.Add("Viudo (a)");
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

            applicantDTO.RequestPhoto = dataUrl;

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

    private async Task SubmitAsync()
    {
        loading = true;
        try
        {
            if (!string.IsNullOrEmpty(applicantDTO.Dpi))
                applicantDTO.Dpi = applicantDTO.Dpi.Replace("-", "");

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