using AdoptaYA.Functionalities.Usuarios.Http;
using AdoptaYA.Functionalities.Usuarios.Model;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdoptaYA.Functionalities.Usuarios.Components;
public partial class ManageUser
{
    [Inject] UsuariosHttp UsuariosHttp { get; set; } = default!;
    [Inject] NotificationService NotificationService { get; set; } = default!;

    [Parameter] public int UserId { get; set; }

    private bool loading = false;

    private UsuarioResponseDTO User = new UsuarioResponseDTO();
    private IList<RolResponseDTO> Roles = new List<RolResponseDTO>();

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        User = await UsuariosHttp.GetUserById(UserId);
        Roles = await UsuariosHttp.GetAllRoles();
        loading = false;

        StateHasChanged();

    }

    private async Task SetRolToUser(int id_rol, int id_user)
    {
        loading = true;

        RolUsuarioRequestDTO dto = new RolUsuarioRequestDTO { RolId = id_rol, UserId = id_user };
        var response = await UsuariosHttp.CreateRolUsuario(dto);

        if (response)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = "Rol asignado",
                Detail = "El rol se asignó correctamente al usuario",
                Duration = 3000
            });
        }

        loading = false;

        await LoadDataAsync();

    }

}