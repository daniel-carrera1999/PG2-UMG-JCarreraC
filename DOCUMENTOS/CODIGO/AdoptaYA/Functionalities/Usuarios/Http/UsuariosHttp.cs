
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Services.Http;
using System.Text.Json;
using AdoptaYA.Functionalities.Modules.Model;
using global::AdoptaYA.Services.Dialogs;
using global::AdoptaYA.Services.Http;
using AdoptaYA.Functionalities.Usuarios.Model;

namespace AdoptaYA.Functionalities.Usuarios.Http;
public class UsuariosHttp
{
    private readonly ApiGetService _apiGetService;
    private readonly ApiPostService _apiPostService;
    private readonly ApiDeleteService _apiDeleteService;
    private readonly ApiPutService _apiPutService;
    private readonly CustomDialogService _customDialogService;

    public UsuariosHttp(ApiGetService apiGetService,
                       ApiPostService apiPostService,
                       ApiDeleteService apiDeleteService,
                       ApiPutService apiPutService,
                       CustomDialogService customDialogService)
    {
        _apiGetService = apiGetService;
        _apiPostService = apiPostService;
        _apiDeleteService = apiDeleteService;
        _apiPutService = apiPutService;
        _customDialogService = customDialogService;
    }

    public async Task<IList<UsuarioResponseDTO>> GetAllUsers()
    {
        try
        {
            var response = await _apiGetService.GetAsync("usuario", "", 1, true);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<UsuarioResponseDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<UsuarioResponseDTO>();

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

        return new List<UsuarioResponseDTO>();
    }

    public async Task<UsuarioResponseDTO> GetUserById(int id)
    {
        try
        {
            var response = await _apiGetService.GetAsync($"usuario/{id}", "", 1, true);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<UsuarioResponseDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new UsuarioResponseDTO();

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

        return new UsuarioResponseDTO();
    }

    public async Task<IList<RolResponseDTO>> GetAllRoles()
    {
        try
        {
            var response = await _apiGetService.GetAsync("rol", "", 1, true);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<RolResponseDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<RolResponseDTO>();

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

        return new List<RolResponseDTO>();
    }

    public async Task<bool> CreateRolUsuario(RolUsuarioRequestDTO dto)
    {
        try
        {
            var response = await _apiPostService.PostAsync("rol_usuario", dto, 1, true);

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