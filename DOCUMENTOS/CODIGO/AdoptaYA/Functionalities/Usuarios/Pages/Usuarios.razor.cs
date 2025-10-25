using AdoptaYA.Functionalities.Usuarios.Components;
using AdoptaYA.Functionalities.Usuarios.Http;
using AdoptaYA.Functionalities.Usuarios.Model;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdoptaYA.Functionalities.Usuarios.Pages;
public partial class Usuarios
{
    [Inject] UsuariosHttp UsuariosHttp { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;

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

}