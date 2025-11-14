using AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
using AdoptaYA.Functionalities.AdoptionRequestManagement.Http;
using AdoptaYA.Functionalities.AdoptionRequestManagement.Model;
using AdoptaYA.Functionalities.MyAdoptionRequests.Model;
using AdoptaYA.Services.Security;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.Components;
public partial class AdoptionRequestManage
{
    [Inject] AdoptionRequestManagementHttp AdoptionRequestManagementHttp { get; set; } = default!;
    [Inject] GetCurrentUser GetCurrentUser { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;
    [Parameter] public int RequestId { get; set; }

    private bool loading = false;

    private AdoptionRootResponse data = new();

    private RadzenDataGrid<EnfermedadResponse> diseasesGrid;
    void RowRenderDiseases(RowRenderEventArgs<EnfermedadResponse> args)
    {
        args.Expandable = args.Data.Medicines != null && args.Data.Medicines.Any();
    }

    private bool applicationCompleted = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        data = await AdoptionRequestManagementHttp.GetAdoptionRequestDetail(RequestId);
        applicationCompleted = data.Adoption!.Status switch
        {
            "Pendiente" => false,
            "En Proceso" => false,
            "Aprobada" => false,
            "Completada" => true,
            "Cancelada" => true,
            "En Seguimiento" => false,
            _ => false
        };
        loading = false;

        StateHasChanged();
    }

    private bool isEditingStatus = false;
    private string selectedStatus = string.Empty;
    private List<string> statusList = new List<string>
    {
        "Pendiente",
        "En Proceso",
        "Aprobada",
        "Completada",
        "Cancelada",
        "En Seguimiento"
    };

    private void StartEditStatus()
    {
        isEditingStatus = true;
        selectedStatus = data?.Adoption?.Status ?? "Pendiente";
        StateHasChanged();
    }

    private void CancelEditStatus()
    {
        isEditingStatus = false;
        selectedStatus = string.Empty;
        StateHasChanged();
    }

    private async void SaveStatus()
    {
        loading = true;

        var user = await GetCurrentUser.GetUserInfoAsync();

        UpdateAdoptionRequestStatusDTO dto = new();
        dto.UserId = user.UserId ?? 0;
        dto.Status = selectedStatus;

        await AdoptionRequestManagementHttp.UpdateAdoptionRequestStatus(data.Adoption!.Id, dto);

        loading = false;

        isEditingStatus = false;
        await LoadDataAsync();

        StateHasChanged();
    }

    private async Task OpenAddAdoptionFollowUp(int id)
    {
        var user = await GetCurrentUser.GetUserInfoAsync();
        var response = await DialogService.OpenAsync<AddAdoptionFollowUp>(
            $"Solicitud ID: <strong>{id}</strong>",
            new Dictionary<string, object?>
            {
                { "AdoptionId", id },
                { "UserId", user.UserId ?? 0 }
            },
            new DialogOptions { Width = "75%", ShowClose = false, CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false }
        );
        if (response)
        {
            await LoadDataAsync();
        }
    }

    private async Task OpenAddAdoptionReturn(int id)
    {
        var user = await GetCurrentUser.GetUserInfoAsync();
        var response = await DialogService.OpenAsync<AddAdoptionReturn>(
            $"Solicitud ID: <strong>{id}</strong>",
            new Dictionary<string, object?>
            {
                { "AdoptionId", id },
                { "UserId", user.UserId ?? 0 }
            },
            new DialogOptions { Width = "75%", ShowClose = false, CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false }
        );
        if (response)
        {
            await LoadDataAsync();
        }
    }

    private async Task DownloadAdoptionRequestPdf()
    {
        loading = true;
        await AdoptionRequestManagementHttp.GetAdoptionRequestDownloadAsync(data.Adoption!.Id);
        loading = false;

        StateHasChanged();
    }

    private void Close()
    {
        DialogService.Close();
    }

}