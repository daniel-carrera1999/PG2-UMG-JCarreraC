using AdoptaYA.Functionalities.Animals.Model;
using System.Text.Json;
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Services.Http;
using AdoptaYA.Functionalities.Pets.Model;
using AdoptaYA.Functionalities.Pets.Components;

namespace AdoptaYA.Functionalities.Pets.Http;
public class PetsHttp
{
    private readonly ApiGetService _apiGetService;
    private readonly ApiPostService _apiPostService;
    private readonly ApiDeleteService _apiDeleteService;
    private readonly ApiPutService _apiPutService;
    private readonly CustomDialogService _customDialogService;
    private readonly ILogger<AddPet> _addPetLogger;

    public PetsHttp(ApiGetService apiGetService,
                    ApiPostService apiPostService,
                    ApiDeleteService apiDeleteService,
                    ApiPutService apiPutService,
                    CustomDialogService customDialogService,
                    ILogger<AddPet> addPetLogger)
    {
        _apiGetService = apiGetService;
        _apiPostService = apiPostService;
        _apiDeleteService = apiDeleteService;
        _apiPutService = apiPutService;
        _customDialogService = customDialogService;
        _addPetLogger = addPetLogger;
    }

    public async Task<IList<PetsView>> GetAllPets()
    {
        try
        {
            var response = await _apiGetService.GetAsync("mascota?inactive=0", "", 1, false);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<PetsView>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<PetsView>();

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

        return new List<PetsView>();
    }

    public async Task<MascotaResponseDTO> GetPetAsync(int id)
    {
        try
        {
            var response = await _apiGetService.GetAsync($"mascota/{id}", "", 1, true);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<MascotaResponseDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new MascotaResponseDTO();

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

        return new MascotaResponseDTO();
    }

    public async Task<PetPhotosView> GetPetPhotos(int id, bool all)
    {
        try
        {
            var allPhotos = all ? "true" : "false";
            var response = await _apiGetService.GetAsync($"mascota/{id}/photos/{allPhotos}", "", 1, false);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<PetPhotosView>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new PetPhotosView();

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

        return new PetPhotosView();
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

    public async Task<bool> CreatePet(MascotaRequestDTO pet)
    {
        try
        {
            var response = await _apiPostService.PostAsync("mascota", pet, 1, false);

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

    public async Task<bool> UpdatePet(int id, MascotaRequestDTO pet)
    {
        try
        {
            var response = await _apiPutService.PutAsync($"mascota/{id}", pet, 1, false);

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