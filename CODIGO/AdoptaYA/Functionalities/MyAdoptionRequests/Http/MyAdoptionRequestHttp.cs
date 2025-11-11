using System.Text.Json;
using AdoptaYA.Functionalities.MyAdoptionRequests.Model;
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Services.Http;

namespace AdoptaYA.Functionalities.MyAdoptionRequests.Http;
public class MyAdoptionRequestHttp
{
    private readonly ApiGetService _apiGetService;
    private readonly ApiPostService _apiPostServie;
    private readonly ApiDeleteService _apiDeleteService;
    private readonly ApiPutService _apiPutService;
    private readonly CustomDialogService _customDialogService;

    public MyAdoptionRequestHttp(ApiGetService apiGetService,
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

    public async Task<IList<AdoptionRequestResponse>> GetMyAdoptionRequest(int UserId)
    {
        try
        {
            var response = await _apiGetService.GetAsync($"adopcion/mis_solicitudes/{UserId}", "", 1, false);


            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<AdoptionRequestResponse>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<AdoptionRequestResponse>();

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

        return new List<AdoptionRequestResponse>();
    }

    public async Task<AdoptionRequestDetailResponse> GetMyAdoptionRequestById(int RequestId)
    {
        try
        {
            var response = await _apiGetService.GetAsync($"adopcion/detalle_solicitud/{RequestId}", "", 1, true);


            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<AdoptionRequestDetailResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new AdoptionRequestDetailResponse();

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

        return new AdoptionRequestDetailResponse();
    }

}