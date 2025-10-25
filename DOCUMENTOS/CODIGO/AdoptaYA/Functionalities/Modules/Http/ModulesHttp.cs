using AdoptaYA.Services.Dialogs;
using AdoptaYA.Services.Http;
using System.Text.Json;
using AdoptaYA.Functionalities.Modules.Model;

namespace AdoptaYA.Functionalities.Modules.Http;
public class ModulesHttp
{
    private readonly ApiGetService _apiGetService;
    private readonly ApiPostService _apiPostService;
    private readonly ApiDeleteService _apiDeleteService;
    private readonly ApiPutService _apiPutService;
    private readonly CustomDialogService _customDialogService;

    public ModulesHttp(ApiGetService apiGetService,
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

    public async Task<IList<Modulo>> GetAllModules()
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

    public async Task<bool> CreateModule(Modulo module)
    {
        try
        {
            var response = await _apiPostService.PostAsync("modulo", module, 1, true);

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

    public async Task<bool> UpdateModule(Modulo module)
    {
        try
        {
            var response = await _apiPutService.PutAsync($"modulo/{module.Id}", module, 1, true);
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

    public async Task<bool> DeleteModule(int id)
    {
        try
        {
            var response = await _apiDeleteService.DeleteAsync("modulo", id, 1, true);

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