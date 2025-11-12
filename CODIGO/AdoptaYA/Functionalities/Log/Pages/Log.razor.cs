using AdoptaYA.Functionalities.Log.Model;
using AdoptaYA.Functionalities.Log.Http;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System.Text;
using Microsoft.JSInterop;

namespace AdoptaYA.Functionalities.Log.Pages;
public partial class Log{

    [Inject] BitacoraHttp BitacoraHttp { get; set; } = default!;
    [Inject] IJSRuntime JS { get; set; } = default!;

    private bool loading = false;

    private IList<Bitacora> logs = new List<Bitacora>();
    RadzenDataGrid<Bitacora> grid = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        logs = await BitacoraHttp.GetAllLogs();
        loading = false;

        StateHasChanged();
    }

    private async Task ExportToCsv()
    {
        if (logs == null || !logs.Any())
            return;

        var csv = new StringBuilder();
        csv.AppendLine("Id,Tabla,Accion,Fecha,Datos,Usuario");

        foreach (var log in logs)
        {
            csv.AppendLine($"{log.Id},\"{log.Table}\",\"{log.Action}\",{log.Date:yyyy-MM-dd HH:mm:ss},\"{log.Data}\",\"{log.User?.UserName}\"");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        var base64 = Convert.ToBase64String(bytes);
        var fileName = $"Bitacora_{DateTime.Now:yyyyMMddHHmmss}.csv";

        await JS.InvokeVoidAsync("downloadFileFromBase64", fileName, "text/csv", base64);
    }

}