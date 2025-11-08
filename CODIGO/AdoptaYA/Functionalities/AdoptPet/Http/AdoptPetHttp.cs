using System.Text.Json;
using AdoptaYA.Functionalities.AdoptPet.Model;
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Services.Http;

namespace AdoptaYA.Functionalities.AdoptPet.Http;
public class AdoptPetHttp
{
    private readonly ApiGetService _apiGetService;
    private readonly ApiPostService _apiPostService;
    private readonly ApiDeleteService _apiDeleteService;
    private readonly ApiPutService _apiPutService;
    private readonly CustomDialogService _customDialogService;

    public AdoptPetHttp(ApiGetService apiGetService,
                        ApiPostService apiPostService,
                        ApiDeleteService apiDeleteService,
                        ApiPutService apiPutService,
                        CustomDialogService customDialogService
        )
    {
        _apiGetService = apiGetService;
        _apiPostService = apiPostService;
        _apiDeleteService = apiDeleteService;
        _apiPutService = apiPutService;
        _customDialogService = customDialogService;
    }

    public async Task<IList<AvailablePet>> GetAllAvailablePets()
    {
        try
        {
            var response = await _apiGetService.GetAsync("adopcion/mascotas-disponibles", "", 1, true);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<AvailablePet>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<AvailablePet>();

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

        return new List<AvailablePet>();
    }

    public async Task<bool> CreateAdoption(AdoptionRequestDTO dto)
    {
        try
        {
            var response = await _apiPostService.PostAsync("adopcion/adoptar", dto, 1, true);

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