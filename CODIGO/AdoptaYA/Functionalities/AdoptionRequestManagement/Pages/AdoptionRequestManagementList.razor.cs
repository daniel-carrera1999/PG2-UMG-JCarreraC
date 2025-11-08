using AdoptaYA.Functionalities.AdoptionRequestManagement.Components;
using AdoptaYA.Functionalities.AdoptionRequestManagement.Http;
using AdoptaYA.Functionalities.AdoptionRequestManagement.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System.Text;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.Pages;
public partial class AdoptionRequestManagementList
{
    [Inject] AdoptionRequestManagementHttp Http { get; set; } = default!;
    [Inject] IJSRuntime JS { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;

    private bool loading = false;

    private IList<AdoptionRequestView> data = new List<AdoptionRequestView>();

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        data = await Http.GetAllAdoptionRequest();
        loading = false;

        StateHasChanged();
    }

    private async Task OpenViewRequest(int id)
    {
        var response = await DialogService.OpenAsync<AdoptionRequestManage>(
            $"Solicitud ID: <strong>{id}</strong>",
            new Dictionary<string, object?>
            {
                { "RequestId", id }
            },
            new DialogOptions { Width = "75%", ShowClose = false, CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false }
        );
    }

    private async Task ExportToCsv()
    {
        if (data == null || !data.Any())
            return;

        var csv = new StringBuilder();
        csv.AppendLine("Id,Nombre,Especie,Raza,Estado,Fecha solicitud");

        foreach (var rq in data)
        {
            csv.AppendLine($"{rq.AdoptionId},\"{rq.Name}\",\"{rq.Species}\",\"{rq.Breed}\",\"{rq.Status}\",{rq.Date:yyyy-MM-dd HH:mm:ss}");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        var base64 = Convert.ToBase64String(bytes);
        var fileName = $"Gestion de adopciones_{DateTime.Now:yyyyMMddHHmmss}.csv";

        await JS.InvokeVoidAsync("downloadFileFromBase64", fileName, "text/csv", base64);
    }

}