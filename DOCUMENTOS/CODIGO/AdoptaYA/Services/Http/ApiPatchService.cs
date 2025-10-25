using System.Net.Http.Headers;
using AdoptaYA.Services.Security;
using AdoptaYA.Shared.Interfaces;
using AdoptaYA.Shared.Model;
using Microsoft.Extensions.Options;

namespace AdoptaYA.Services.Http;
public class ApiPatchService : IApiPatchService
{
    private readonly HttpClient _httpClient;
    private readonly Uri _coreApiUrl;
    private readonly Uri _uniUserAuthApiUrl;
    private readonly Uri _uniUserClientApiUrl;
    private readonly Uri _ubicacionesApiUrl;
    private readonly ILogger<ApiPostService> _logger;
    private readonly AccessControlService _accessControlService;
    private readonly GetCurrentUser _getCurrentUser;

    public ApiPatchService(HttpClient httpClient,
                           IOptions<ApiSettings> settings,
                           ILogger<ApiPostService> logger,
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

    public async Task<HttpResponseMessage> PatchAsync<TRequest>(string url, TRequest? data = default, int source = 1, bool log = false)
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

            var requestUri = new Uri(baseUri, url);
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUri);

            if (data is not null)
            {
                request.Content = JsonContent.Create(data);
            }

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
                _logger.LogWarning("---------- API POST RESPONSE RAW ----------");
                _logger.LogWarning("Request to: {Url}", requestUri);
                _logger.LogWarning("HTTP Status: {Code} - {Reason}", response.StatusCode, response.ReasonPhrase);
                _logger.LogWarning("Response JSON (raw):");
                _logger.LogWarning(string.IsNullOrWhiteSpace(rawContent) ? "[VACÍO]" : rawContent);
                _logger.LogWarning("--------------------------------------------");
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

    public async Task<HttpResponseMessage> PatchWithoutBodyRequestAsync(string url, int source = 1, bool log = false)
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

            var requestUri = new Uri(baseUri, url);
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUri);

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
                _logger.LogWarning("---------- API POST RESPONSE RAW ----------");
                _logger.LogWarning("Request to: {Url}", requestUri);
                _logger.LogWarning("HTTP Status: {Code} - {Reason}", response.StatusCode, response.ReasonPhrase);
                _logger.LogWarning("Response JSON (raw):");
                _logger.LogWarning(string.IsNullOrWhiteSpace(rawContent) ? "[VACÍO]" : rawContent);
                _logger.LogWarning("--------------------------------------------");
            }

            return response;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning("La solicitud fue cancelada (posible redirección o cierre de sesión): {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción inesperada durante PostAsync");
            _logger.LogError(ex.Message);
            throw;
        }
    }
}