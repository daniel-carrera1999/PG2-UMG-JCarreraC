using AdoptaYA.Functionalities.Permissions.Http;
using AdoptaYA.Functionalities.Permissions.Model;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AdoptaYA.Functionalities.Permissions.Components;
public partial class PermissionsDialog
{
    [Inject] RolesAndPermissionHttp RolesAndPermissionHttp { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;

    [Parameter] public int RolId { get; set; }

    private Rol Rol = new Rol();
    private IList<Modulo> Modulos = new List<Modulo>();
    private IList<ModuloPermisoViewModel> MatrizPermisos = new List<ModuloPermisoViewModel>();

    private Permiso newPermiso = new();

    private RadzenDataGrid<ModuloPermisoViewModel>? grid;

    private bool loading = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            loading = true;
            await LoadDataAsync();
            loading = false;

            StateHasChanged();
        }
    }

    private async Task LoadDataAsync()
    {
        Rol = await RolesAndPermissionHttp.GetRoleById(RolId);
        Modulos = await RolesAndPermissionHttp.GetAllModulos();

        foreach (var modulo in Modulos)
        {
            var permisoExistente = Rol!.Permissions!.FirstOrDefault(p => p.ModuleId == modulo.Id);

            MatrizPermisos.Add(new ModuloPermisoViewModel
            {
                IdModulo = modulo.Id,
                Name = modulo.Name!,
                Path = modulo.Path!,
                Icon = modulo.Icon!,
                Description = modulo.Description!,

                Create = permisoExistente?.Create == 1,
                Read = permisoExistente?.Read == 1,
                Update = permisoExistente?.Update == 1,
                Delete = permisoExistente?.Delete == 1,
                IsCreated = permisoExistente != null
            });
        }

        await grid!.Reload();
    }

    private async Task SetPermissionAsync(ModuloPermisoViewModel item, bool newValue)
    {
        loading = true;

        newPermiso.ModuleId = item.IdModulo;
        newPermiso.RoleId = Rol!.Id;
        newPermiso.Create = item.Create ? 1 : 0;
        newPermiso.Read = item.Read ? 1 : 0;
        newPermiso.Update = item.Update ? 1 : 0;
        newPermiso.Delete = item.Delete ? 1 : 0;

        bool UpdateOrDelete = item.Create || item.Read || item.Update || item.Delete;

        if (newValue)
        {
            if (!item.IsCreated)
            {
                await RolesAndPermissionHttp.CreatePermission(newPermiso);
            } 
            else
            {
                await RolesAndPermissionHttp.UpdatePermission(newPermiso);
            }
        }
        else
        {
            if (item.IsCreated && UpdateOrDelete)
            {
                await RolesAndPermissionHttp.UpdatePermission(newPermiso);
            }
            else
            {
                await RolesAndPermissionHttp.DeletePermission(newPermiso);
            }
        }

        newPermiso = new();

        loading = false;
    }

}