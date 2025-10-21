using System.Text.Json;
using AdoptaYA.Functionalities.Animals.Model;
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Services.Http;

namespace AdoptaYA.Functionalities.Animals.Http;
public class AnimalHttp
{
    private readonly ApiGetService _apiGetService;
    private readonly ApiPostService _apiPostService;
    private readonly ApiDeleteService _apiDeleteService;
    private readonly ApiPutService _apiPutService;
    private readonly CustomDialogService _customDialogService;

    public AnimalHttp(ApiGetService apiGetService,
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

    public async Task<IList<Animal>> GetAllAnimals()
    {
        try
        {
            var response = await _apiGetService.GetAsync("animal?inactive=0", "", 1, true);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<Animal>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<Animal>();

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

        return new List<Animal>();
    }

    public async Task<bool> CreateAnimal(Animal animal)
    {
        try
        {
            var response = await _apiPostService.PostAsync("animal", animal, 1, true);

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

    public async Task<bool> UpdateAnimal(Animal animal)
    {
        try
        {
            var response = await _apiPutService.PutAsync($"animal/{animal.Id}", animal, 1, true);
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

    public async Task<bool> DeleteAnimal(int id)
    {
        try
        {
            var response = await _apiDeleteService.DeleteAsync("animal", id, 1, true);

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