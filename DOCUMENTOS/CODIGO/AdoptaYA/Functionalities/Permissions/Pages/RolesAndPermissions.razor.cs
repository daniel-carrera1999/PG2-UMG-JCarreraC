using AdoptaYA.Functionalities.Modules.Http;
using AdoptaYA.Functionalities.Permissions.Components;
using AdoptaYA.Functionalities.Permissions.Http;
using AdoptaYA.Functionalities.Permissions.Model;
using AdoptaYA.Services.Dialogs;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdoptaYA.Functionalities.Permissions.Pages;
public partial class RolesAndPermissions
{
    [Inject] RolesAndPermissionHttp rolesHttp { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;
    [Inject] CustomDialogService customDialogService { get; set; } = default!;

    bool loading = false;

    private bool isEdit = false;
    private Rol newRol = new Rol();
    private IList<Rol> roles = new List<Rol>();

    protected override async void OnInitialized()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        roles = await rolesHttp.GetAllRoles();
        loading = false;
        StateHasChanged();
    }

    private void SetUpdate(Rol rol)
    {
        isEdit = true;
        newRol = new Rol
        {
            Id = rol.Id,
            Name = rol.Name,
            Description = rol.Description
        };
        StateHasChanged();
    }

    private void CancelUpdate()
    {
        isEdit = false;
        newRol = new Rol();
        StateHasChanged();
    }

    private async Task AddRol()
    {
        bool run = await customDialogService.OpenConfirmAsync("¿Está a punto de crear un nuevo rol, desea continuar?", "Confirmar", "Continuar", "Cancelar", new DialogOptions { CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false, ShowClose = false });

        if (run)
        {
            loading = true;
            bool response = await rolesHttp.CreateRol(newRol);
            loading = false;

            if (response)
            {
                newRol = new Rol();
                await LoadDataAsync();
            }
        }
    }

    private async Task UpdateRol()
    {
        bool run = await customDialogService.OpenConfirmAsync($"¿Está a punto de modificar el rol {newRol.Name}, desea continuar?", "Confirmar", "Continuar", "Cancelar", new DialogOptions { CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false, ShowClose = false });

        if (run)
        {
            loading = true;
            bool response = await rolesHttp.UpdateRol(newRol);
            loading = false;

            if (response)
            {
                newRol = new Rol();
                await LoadDataAsync();
            }
        }
    }

    private bool IsFormIncomplete =>
        string.IsNullOrWhiteSpace(newRol?.Name) ||
        string.IsNullOrWhiteSpace(newRol?.Description);

    private async Task DeleteRol(int rolId)
    {
        bool run = await customDialogService.OpenConfirmAsync($"¿Está a punto de eliminar el rol con id {rolId}, desea continuar?", "Confirmar", "Continuar", "Cancelar", new DialogOptions { CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false, ShowClose = false });

        if (run)
        {
            loading = true;
            bool response = await rolesHttp.DeleteRol(rolId);
            loading = false;

            if (response)
            {
                newRol = new Rol();
                await LoadDataAsync();
            }

        }
    }

    public async Task OpenManagePermissions(Rol rol)
    {
        var response = await DialogService.OpenAsync<PermissionsDialog>(
            $"Gestionar permisos rol: <strong>{rol.Id}</strong>",
            new Dictionary<string, object?>
            {
                { "RolId", rol.Id }
            },
            new DialogOptions { Width = "75%" }
        );
    }
}