using AdoptaYA.Services.Security;
using AdoptaYA.Shared.Model;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace AdoptaYA.Services.Http;
public class ApiHeadService
{
    private readonly HttpClient _httpClient;
    private readonly Uri _coreApiUrl;
    private readonly Uri _uniUserAuthApiUrl;
    private readonly Uri _uniUserClientApiUrl;
    private readonly Uri _ubicacionesApiUrl;
    private readonly ILogger<ApiHeadService> _logger;
    private readonly GetCurrentUser _getCurrentUser;
    private readonly AccessControlService _accessControlService;

    public ApiHeadService(HttpClient httpClient,
                          IOptions<ApiSettings> settings,
                          ILogger<ApiHeadService> logger,
                          GetCurrentUser getCurrentUser,
                          AccessControlService accessControlService)
    {
        var options = settings.Value;
        _httpClient = httpClient;
        _coreApiUrl = new Uri(options.CoreApiUrl);
        _uniUserAuthApiUrl = new Uri(options.UniUserAuthApiUrl);
        _uniUserClientApiUrl = new Uri(options.UniUserClientApiUrl);
        _ubicacionesApiUrl = new Uri(options.UbicacionesApiUrl);
        _logger = logger;
        _getCurrentUser = getCurrentUser;
        _accessControlService = accessControlService;
    }

    public async Task<HttpResponseMessage> HeadAsync(string url, int source = 1, bool log = false)
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
            var request = new HttpRequestMessage(HttpMethod.Head, requestUri);

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
                _logger.LogWarning("---------- API HEAD RESPONSE RAW ----------");
                _logger.LogWarning("Request to: {Url}", requestUri);
                _logger.LogWarning("HTTP Status: {Code} - {Reason}", response.StatusCode, response.ReasonPhrase);
                _logger.LogWarning("Response JSON (raw):");
                _logger.LogWarning(string.IsNullOrWhiteSpace(rawContent) ? "[VACÍO]" : rawContent);
                _logger.LogWarning("-------------------------------------------");
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