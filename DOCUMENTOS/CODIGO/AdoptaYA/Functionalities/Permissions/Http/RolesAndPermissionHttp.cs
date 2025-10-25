using System.Text.Json;
using AdoptaYA.Functionalities.Permissions.Model;
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Services.Http;

namespace AdoptaYA.Functionalities.Permissions.Http;
public class RolesAndPermissionHttp
{

    private readonly ApiGetService _apiGetService;
    private readonly ApiPostService _apiPostServie;
    private readonly ApiDeleteService _apiDeleteService;
    private readonly ApiPutService _apiPutService;
    private readonly CustomDialogService _customDialogService;

    public RolesAndPermissionHttp(ApiGetService apiGetService,
                                 ApiPostService apiPostService,
                                 ApiDeleteService apiDeleteService,
                                 ApiPutService apiPutService,
                                 CustomDialogService customDialogService)
    {
        _apiGetService = apiGetService;
        _apiPostServie = apiPostService;
        _apiDeleteService = apiDeleteService;
        _apiPutService = apiPutService;
        _customDialogService = customDialogService;
    }

    public async Task<IList<Rol>> GetAllRoles()
    {
        try
        {
            var response = await _apiGetService.GetAsync("rol", "", 1, true);


            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<Rol>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<Rol>();

            }
            else
            {
                await _customDialogService.OpenViewErrorsAsync(response);
            }
        }
        catch (Exception e)
        {
            await _customDialogService.OpenInternalErrorAsync(e);
        }

        return new List<Rol>();
    }

    public async Task<Rol> GetRoleById(int id)
    {
        try
        {
            var response = await _apiGetService.GetAsync($"rol/{id}", "", 1, true);


            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<Rol>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new Rol();

            }
            else
            {
                await _customDialogService.OpenViewErrorsAsync(response);
            }
        }
        catch (Exception e)
        {
            await _customDialogService.OpenInternalErrorAsync(e);
        }

        return new Rol();
    }

    public async Task<bool> CreateRol(Rol rol)
    {
        try
        {
            var response = await _apiPostServie.PostAsync("rol", rol, 1, true);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                await _customDialogService.OpenViewErrorsAsync(response);
            }
        }
        catch (Exception e)
        {
            await _customDialogService.OpenInternalErrorAsync(e);
        }
        return false;
    }

    public async Task<bool> UpdateRol(Rol rol)
    {
        try
        {
            var response = await _apiPutService.PutAsync($"rol/{rol.Id}", rol, 1, true);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                await _customDialogService.OpenViewErrorsAsync(response);
            }
        }
        catch (Exception e)
        {
            await _customDialogService.OpenInternalErrorAsync(e);
        }
        return false;
    }

    public async Task<bool> DeleteRol(int id)
    {
        try
        {
            var response = await _apiDeleteService.DeleteAsync("rol", id, 1, true);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                await _customDialogService.OpenViewErrorsAsync(response);
            }
        }
        catch (Exception e)
        {
            await _customDialogService.OpenInternalErrorAsync(e);
        }
        return false;
    }

    public async Task<IList<Modulo>> GetAllModulos()
    {
        try
        {
            var response = await _apiGetService.GetAsync("modulo", "", 1, true);


            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<Modulo>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<Modulo>();

            }
            else
            {
                await _customDialogService.OpenViewErrorsAsync(response);
            }
        }
        catch (Exception e)
        {
            await _customDialogService.OpenInternalErrorAsync(e);
        }

        return new List<Modulo>();
    }
    public async Task<bool> CreatePermission(Permiso permiso)
    {
        try
        {
            var response = await _apiPostServie.PostAsync("permiso", permiso, 1, true);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                await _customDialogService.OpenViewErrorsAsync(response);
            }
        }
        catch (Exception e)
        {
            await _customDialogService.OpenInternalErrorAsync(e);
        }
        return false;
    }

    public async Task<bool> UpdatePermission(Permiso permiso)
    {
        try
        {
            var response = await _apiPutService.PutAsync($"permiso/id_rol/{permiso.RoleId}/id_modulo/{permiso.ModuleId}", permiso, 1, true);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                await _customDialogService.OpenViewErrorsAsync(response);
            }
        }
        catch (Exception e)
        {
            await _customDialogService.OpenInternalErrorAsync(e);
        }
        return false;
    }

    public async Task<bool> DeletePermission(Permiso permiso)
    {
        try
        {
            var response = await _apiDeleteService.DeleteMultiIdKeyAsync($"permiso/id_rol/{permiso.RoleId}/id_modulo/{permiso.ModuleId}", 1, true);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                await _customDialogService.OpenViewErrorsAsync(response);
            }
        }
        catch (Exception e)
        {
            await _customDialogService.OpenInternalErrorAsync(e);
        }
        return false;
    }
}
