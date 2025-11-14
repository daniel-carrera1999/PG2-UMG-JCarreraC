using AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
using AdoptaYA.Functionalities.AdoptionRequestManagement.Model;
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Services.Http;
using Microsoft.JSInterop;
using Radzen;
using System.Text.Json;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.Http;
public class AdoptionRequestManagementHttp
{
    private readonly ApiGetService _apiGetService;
    private readonly ApiPostService _apiPostServie;
    private readonly ApiDeleteService _apiDeleteService;
    private readonly ApiPutService _apiPutService;
    private readonly CustomDialogService _customDialogService;
    private readonly IJSRuntime _jSRuntime;
    private readonly DialogService _dialogService;

    public AdoptionRequestManagementHttp(ApiGetService apiGetService,
                                         ApiPostService apiPostService,
                                         ApiDeleteService apiDeleteService,
                                         ApiPutService apiPutService,
                                         CustomDialogService customDialogService,
                                         IJSRuntime jSRuntime,
                                         DialogService dialogService)
    {
        _apiGetService = apiGetService;
        _apiPostServie = apiPostService;
        _apiDeleteService = apiDeleteService;
        _apiPutService = apiPutService;
        _customDialogService = customDialogService;
        _jSRuntime = jSRuntime;
        _dialogService = dialogService;
    }

    public async Task<IList<AdoptionRequestView>> GetAllAdoptionRequest()
    {
        try
        {
            var response = await _apiGetService.GetAsync("adopcion/todas_solicitudes", "", 1, false);


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

    public async Task<bool> UpdateAdoptionRequestStatus(int id, UpdateAdoptionRequestStatusDTO dto)
    {
        try
        {
            var response = await _apiPutService.PutAsync($"adopcion/{id}/status", dto, 1, false);

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

    public async Task<bool> AddAdopionFollowUp(AdoptionRequestFollowUpDTO dto)
    {
        try
        {
            var response = await _apiPostServie.PostAsync($"adopcion/seguimiento", dto, 1, false);

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

    public async Task<bool> AddAdoptionReturn(AdoptionRequestReturnDTO dto)
    {
        try
        {
            var response = await _apiPostServie.PostAsync($"adopcion/retorno", dto, 1, false);

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

    public async Task GetAdoptionRequestDownloadAsync(int AdoptionId)
    {
        try
        {
            var response = await _apiGetService.GetAsync($"adopcion/{AdoptionId}/pdf", "application/pdf", 1, false);

            if (response.IsSuccessStatusCode)
            {

                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
                var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? "adopt.pdf";

                var base64 = Convert.ToBase64String(fileBytes);

                await _jSRuntime.InvokeVoidAsync("downloadFromByteArray", fileName, base64, contentType);
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
    }

}