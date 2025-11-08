using AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
using AdoptaYA.Functionalities.AdoptionRequestManagement.Http;
using AdoptaYA.Functionalities.MyAdoptionRequests.Model;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.Components;
public partial class AdoptionRequestManage
{
    [Inject] AdoptionRequestManagementHttp AdoptionRequestManagementHttp { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;
    [Parameter] public int RequestId { get; set; }

    private bool loading = false;

    private AdoptionRootResponse data = new();

    private RadzenDataGrid<EnfermedadResponse> diseasesGrid;
    void RowRenderDiseases(RowRenderEventArgs<EnfermedadResponse> args)
    {
        args.Expandable = args.Data.Medicines != null && args.Data.Medicines.Any();
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        data = await AdoptionRequestManagementHttp.GetAdoptionRequestDetail(RequestId);
        loading = false;

        StateHasChanged();
    }

    private void Close()
    {
        DialogService.Close();
    }

}