using AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
using AdoptaYA.Functionalities.AdoptionRequestManagement.Model;
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Services.Http;
using System.Text.Json;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.Http;
public class AdoptionRequestManagementHttp
{
    private readonly ApiGetService _apiGetService;
    private readonly ApiPostService _apiPostServie;
    private readonly ApiDeleteService _apiDeleteService;
    private readonly ApiPutService _apiPutService;
    private readonly CustomDialogService _customDialogService;

    public AdoptionRequestManagementHttp(ApiGetService apiGetService,
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

    public async Task<IList<AdoptionRequestView>> GetAllAdoptionRequest()
    {
        try
        {
            var response = await _apiGetService.GetAsync("adopcion/todas_solicitudes", "", 1, true);


            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<AdoptionRequestView>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<AdoptionRequestView>();

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

        return new List<AdoptionRequestView>();
    }

    public async Task<AdoptionRootResponse> GetAdoptionRequestDetail(int id)
    {
        try
        {
            var response = await _apiGetService.GetAsync($"adopcion/detalle_solicitud_management/{id}", "", 1, true);


            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<AdoptionRootResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new AdoptionRootResponse();

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

        return new AdoptionRootResponse();
    }

}