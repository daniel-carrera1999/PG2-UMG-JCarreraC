using System.Text.Json;
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Services.Http;
using AdoptaYA.Services.Security;
using AdoptaYA.Shared.Model;
using Radzen;

namespace AdoptaYA.Services.SGU;
public class SGUService
{
    private readonly ApiGetService _apiGetService;
    private readonly CustomDialogService _customDialogService;
    private readonly DialogService _dialogService;
    private readonly GetCurrentUser _getCurrentUser;

    public SGUService(ApiGetService apiGetService, CustomDialogService customDialogService, DialogService dialogService, GetCurrentUser getCurrentUser)
    {
        _apiGetService = apiGetService;
        _customDialogService = customDialogService;
        _dialogService = dialogService;
        _getCurrentUser = getCurrentUser;
    }

    public async Task<IList<User>> GetUsersByLocationAsync(int LocationId, bool ExcludeCurrentUser)
    {
        try
        {
            var user = await _getCurrentUser.GetUserInfoAsync();
            string CurrentUser = ExcludeCurrentUser ? $"UserId={user.UserId}" : "";
            var response = await _apiGetService.GetAsync($"ubicaciones/{LocationId.ToString()}/usuarios?{CurrentUser}", "", 3, false);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<User>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<User>();
            }
        }
        catch (Exception e)
        {
            await _dialogService.Alert(e.Message, "Error interno", new AlertOptions() { OkButtonText = "Aceptar" });
        }

        return new List<User>();
    }

    public async Task<IList<User>> GetUsersPhysicalLocationFromUserLocationAsync(int LocationId)
    {
        try
        {
            var user = await _getCurrentUser.GetUserInfoAsync();
            var response = await _apiGetService.GetAsync($"ubicaciones/{LocationId.ToString()}/usuarios?UserId={user.UserId}&BuscarPorUbicacionFisica=true", "", 3, false);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IList<User>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data ?? new List<User>();
            }
        }
        catch (Exception e)
        {
            await _customDialogService.OpenInternalErrorAsync(e);
        }

        return new List<User>();
    }
}