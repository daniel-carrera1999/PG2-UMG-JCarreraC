using System.Text.Json;
using AdoptaYA.Components.Pages;
using AdoptaYA.Functionalities.Pets.Model;
using AdoptaYA.Functionalities.Profile.Model;
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Services.Http;

namespace AdoptaYA.Functionalities.Profile.Http;
public class ProfileHttp
{
    private readonly ApiGetService _apiGetService;
    private readonly ApiPostService _apiPostService;
    private readonly ApiDeleteService _apiDeleteService;
    private readonly ApiPutService _apiPutService;
    private readonly CustomDialogService _customDialogService;

    public ProfileHttp(ApiGetService apiGetService,
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

    public async Task<ProfileResponseDTO> GetProfileAsync(int id)
    {
        try
        {
            var response = await _apiGetService.GetAsync($"usuario/{id}", "", 1, true);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ProfileResponseDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new ProfileResponseDTO();

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

        return new ProfileResponseDTO();
    }

    public async Task<ApplicantResponseDTO> GetApplicantDetailAsync(int id)
    {
        try
        {
            var response = await _apiGetService.GetAsync($"solicitante/{id}", "", 1, true);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ApplicantResponseDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new ApplicantResponseDTO();

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

        return new ApplicantResponseDTO();
    }

    public async Task<bool> CreateApplicant(ApplicantRequestDTO dto)
    {
        try
        {
            var response = await _apiPostService.PostAsync("solicitante", dto, 1, false);

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

    public async Task<bool> UpdateApplicant(int id, ApplicantUpdateRequestDTO dto)
    {
        try
        {
            var response = await _apiPutService.PutAsync($"solicitante/{id}", dto, 1, true);

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

    public async Task<IList<PersonalReferenceResponseDTO>> GetPersonalReferencesByApplicantId(int id)
    {
        try
        {
            var response = await _apiGetService.GetAsync($"referencia_personal/{id}", "", 1, true);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<PersonalReferenceResponseDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<PersonalReferenceResponseDTO>();

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

        return new List<PersonalReferenceResponseDTO>();
    }

    public async Task<bool> UpdatePersonalReferences(int id, IList<PersonalReferenceResponseDTO> dto)
    {
        try
        {
            var response = await _apiPutService.PutAsync($"referencia_personal/{id}", dto, 1, true);

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