using AdoptaYA.Functionalities.MyAdoptionRequests.Http;
using AdoptaYA.Functionalities.MyAdoptionRequests.Model;
using AdoptaYA.Functionalities.Pets.Model;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AdoptaYA.Functionalities.MyAdoptionRequests.Components;
public partial class AdoptionRequestDetail
{
    [Inject] MyAdoptionRequestHttp MyAdoptionRequestHttp { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;
    [Parameter] public int RequestId { get; set; }

    private bool loading = false;

    private AdoptionRequestDetailResponse data;

    private RadzenDataGrid<AdoptionRequestEnfermedad> diseasesGrid;
    void RowRenderDiseases(RowRenderEventArgs<AdoptionRequestEnfermedad> args)
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
        data = await MyAdoptionRequestHttp.GetMyAdoptionRequestById(RequestId);
        loading = false;

        StateHasChanged();
    }

    private void Close()
    {
        DialogService.Close();
    }

}