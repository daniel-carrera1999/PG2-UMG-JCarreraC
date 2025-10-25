using System.Text.Json;
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Services.Http;
using AdoptaYA.Shared.Model;
using Radzen;

namespace AdoptaYA.Services.SGL;
public class SGLService
{
    private readonly ApiGetService _apiGetService;
    private readonly CustomDialogService _customDialogService;
    private readonly DialogService _dialogService;

    public SGLService(ApiGetService apiGetService, CustomDialogService customDialogService, DialogService dialogService)
    {
        _apiGetService = apiGetService;
        _customDialogService = customDialogService;
        _dialogService = dialogService;
    }

    public async Task<IList<Location>> GetLocationsSendPackagesAsync()
    {
        try
        {
            var response = await _apiGetService.GetAsync("ubicaciones?Asignable=true", "", 4, false);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<Location>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<Location>();
            }
            else
            {
                await _customDialogService.OpenViewErrorsAsync(response);
            }
        }
        catch (Exception e)
        {
            await _dialogService.Alert(e.Message, "Error interno", new AlertOptions() { OkButtonText = "Aceptar" });
        }

        return new List<Location>();
    }

    public async Task<IList<Location>> GetLocationsSendConsolidatesAsync()
    {
        try
        {
            var response = await _apiGetService.GetAsync("ubicaciones?TipoListadoUbicaciones=2", "", 4, false);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<Location>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<Location>();
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

        return new List<Location>();
    }

    public async Task<Location> GetLocationByIdAsync(int LocationId)
    {
        try
        {
            var response = await _apiGetService.GetAsync($"ubicaciones/{LocationId}", "", 4, false);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<Location>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new Location();
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

        return new Location();
    }

}