using AdoptaYA.Functionalities.Log.Pages;
using System.Text;
using AdoptaYA.Functionalities.MyAdoptionRequests.Http;
using AdoptaYA.Functionalities.MyAdoptionRequests.Model;
using AdoptaYA.Services.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AdoptaYA.Functionalities.MyAdoptionRequests.Pages;
public partial class MyAdoptionRequestsList
{
    [Inject] GetCurrentUser GetCurrentUser { get; set; } = default!;
    [Inject] MyAdoptionRequestHttp MyAdoptionRequestHttp { get; set; } = default!;
    [Inject] IJSRuntime JS { get; set; } = default!;

    private bool loading = false;

    private IList<AdoptionRequestResponse> data = new List<AdoptionRequestResponse>();

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        var user = await GetCurrentUser.GetUserInfoAsync();
        data = await MyAdoptionRequestHttp.GetMyAdoptionRequest(user.UserId ?? 0);
        loading = false;

        StateHasChanged();
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
        var fileName = $"Mis solicitudes_{DateTime.Now:yyyyMMddHHmmss}.csv";

        await JS.InvokeVoidAsync("downloadFileFromBase64", fileName, "text/csv", base64);
    }
}