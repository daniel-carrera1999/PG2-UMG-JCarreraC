using AdoptaYA.Functionalities.AdoptPet.Model;
using System.Text.Json;
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Services.Http;
using AdoptaYA.Functionalities.Log.Model;

namespace AdoptaYA.Functionalities.Log.Http;
public class BitacoraHttp
{
    private readonly ApiGetService _apiGetService;
    private readonly ApiPostService _apiPostService;
    private readonly ApiDeleteService _apiDeleteService;
    private readonly ApiPutService _apiPutService;
    private readonly CustomDialogService _customDialogService;

    public BitacoraHttp(ApiGetService apiGetService,
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

    public async Task<IList<Bitacora>> GetAllLogs()
    {
        try
        {
            var response = await _apiGetService.GetAsync("bitacora", "", 1, true);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<Bitacora>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<Bitacora>();

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

        return new List<Bitacora>();
    }

}