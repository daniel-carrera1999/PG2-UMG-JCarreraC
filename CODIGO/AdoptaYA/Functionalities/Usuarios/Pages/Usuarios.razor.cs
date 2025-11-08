using AdoptaYA.Functionalities.Pets.Pages;
using System.Text;
using AdoptaYA.Functionalities.Usuarios.Components;
using AdoptaYA.Functionalities.Usuarios.Http;
using AdoptaYA.Functionalities.Usuarios.Model;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.JSInterop;

namespace AdoptaYA.Functionalities.Usuarios.Pages;
public partial class Usuarios
{
    [Inject] UsuariosHttp UsuariosHttp { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;
    [Inject] IJSRuntime JS { get; set; } = default!;

    private bool loading = false;

    private IList<UsuarioResponseDTO> users = new List<UsuarioResponseDTO>();

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        users = await UsuariosHttp.GetAllUsers();
        loading = false;

        StateHasChanged();
    }

    public async Task OpenManageUser(int id)
    {
        var response = await DialogService.OpenAsync<ManageUser>(
            $"Información del usuario id: <strong>{id}</strong>",
            new Dictionary<string, object?>
            {
                { "UserId", id }
            },
            new DialogOptions { Width = "75%" }
        );
    }

    private async Task ExportToCsv()
    {
        if (users == null || !users.Any())
            return;

        var csv = new StringBuilder();
        csv.AppendLine("Id,Usuario,Nombre,Apellido,Email,Estado");

        foreach (var user in users)
        {
            csv.AppendLine($"{user.Id},\"{user.Username}\",\"{user.Name}\",\"{user.LastName}\",\"{user.Email}\",\"{(user.Inactive == 1 ? "Inactivo" : "Activo")}\"");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        var base64 = Convert.ToBase64String(bytes);
        var fileName = $"Usuarios registrados_{DateTime.Now:yyyyMMddHHmmss}.csv";

        await JS.InvokeVoidAsync("downloadFileFromBase64", fileName, "text/csv", base64);
    }

}