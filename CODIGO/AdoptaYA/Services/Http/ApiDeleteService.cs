using AdoptaYA.Shared.Interfaces;
using AdoptaYA.Shared.Model;
using AdoptaYA.Services.Security;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace AdoptaYA.Services.Http;

public class ApiDeleteService : IApiDeleteService
{
    private readonly HttpClient _httpClient;
    private readonly Uri _coreApiUrl;
    private readonly Uri _uniUserAuthApiUrl;
    private readonly Uri _uniUserClientApiUrl;
    private readonly Uri _ubicacionesApiUrl;
    private readonly ILogger<ApiDeleteService> _logger;
    private readonly AccessControlService _accessControlService;
    private readonly GetCurrentUser _getCurrentUser;

    public ApiDeleteService(HttpClient httpClient,
                           IOptions<ApiSettings> settings,
                           ILogger<ApiDeleteService> logger,
                           AccessControlService accessControlService,
                           GetCurrentUser getCurrentUser)
    {
        var options = settings.Value;
        _httpClient = httpClient;
        _logger = logger;
        _coreApiUrl = new Uri(options.CoreApiUrl);
        _uniUserAuthApiUrl = new Uri(options.UniUserAuthApiUrl);
        _uniUserClientApiUrl = new Uri(options.UniUserClientApiUrl);
        _ubicacionesApiUrl = new Uri(options.UbicacionesApiUrl);
        _accessControlService = accessControlService;
        _getCurrentUser = getCurrentUser;
    }

    public async Task<HttpResponseMessage> DeleteAsync(string url, int id, int source = 1, bool log = false)
    {
        try
        {
            var baseUri = source switch
            {
                1 => _coreApiUrl,
                2 => _uniUserAuthApiUrl,
                3 => _uniUserClientApiUrl,
                4 => _ubicacionesApiUrl,
                _ => _coreApiUrl
            };

            // Construir URL con el ID
            var fullUrl = $"{url}/{id}";
            var requestUri = new Uri(baseUri, fullUrl);

            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            request.Headers.UserAgent.ParseAdd("AdoptaYA/1.0");

            if (source != 2)
            {
                await _accessControlService.RefreshTokenIfExpiringAsync();
                var tokenInfo = await _getCurrentUser.GetTokenInfoAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenInfo.AccessToken);
            }

            var response = await _httpClient.SendAsync(request);
            var rawContent = await response.Content.ReadAsStringAsync();

            if (log)
            {
                _logger.LogWarning("---------- API DELETE RESPONSE RAW ----------");
                _logger.LogWarning("Request to: {Url}", requestUri);
                _logger.LogWarning("HTTP Status: {Code} - {Reason}", response.StatusCode, response.ReasonPhrase);
                _logger.LogWarning("Response JSON (raw):");
                _logger.LogWarning(string.IsNullOrWhiteSpace(rawContent) ? "[VACÍO]" : rawContent);
                _logger.LogWarning("---------------------------------------------");
            }

            return response;
        }
        catch (TaskCanceledException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}