using System.Text.Json;
using AdoptaYA.Shared.Interfaces;
using AdoptaYA.Shared.Model;
using Microsoft.Extensions.Options;

namespace AdoptaYA.Services.Http;
public class ApiService
{
    private readonly HttpClient _http;
    private readonly Uri _baseAddress;
    private readonly Uri _baseAddressUniuser;
    private readonly ILogger<ApiService> _logger;

    public ApiService(HttpClient http, IOptions<ApiSettings> settings, ILogger<ApiService> logger)
    {
        var options = settings.Value;
        _http = http;
        _logger = logger;
        _baseAddress = new Uri(options.CoreApiUrl);
        _baseAddressUniuser = new Uri(options.UniUserAuthApiUrl);
    }

    public async Task<(object? data, string? error)> GetAsync<TResponse, TData, TDataError>(
    string url,
    int source = 1,
    bool log = false)
    where TResponse : class, IApiResponse<TData>
    {
        try
        {
            var baseUri = source == 1 ? _baseAddress : _baseAddressUniuser;
            var requestUri = new Uri(baseUri, url);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.UserAgent.ParseAdd("AdoptaYA/1.0");

            var response = await _http.SendAsync(request);
            var rawContent = await response.Content.ReadAsStringAsync();

            if (log)
            {
                _logger.LogWarning("---------- API RESPONSE RAW ----------");
                _logger.LogWarning("Request to: {Url}", requestUri);
                _logger.LogWarning("HTTP Status: {Code} - {Reason}", response.StatusCode, response.ReasonPhrase);
                _logger.LogWarning("Response JSON (raw):");
                _logger.LogWarning(string.IsNullOrWhiteSpace(rawContent) ? "[VACÍO]" : rawContent);
                //_logger.LogWarning(JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
                _logger.LogWarning("---------------------------------------");
            }

            // 200 OK sin contenido
            if (response.IsSuccessStatusCode && string.IsNullOrWhiteSpace(rawContent))
            {
                return (new { StatusCode = (int)response.StatusCode, IsSuccessStatusCode = (bool)response.IsSuccessStatusCode }, null);
            }

            // 200 OK con contenido (respuesta estándar ApiResponse<TData>)
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var fullResponse = JsonSerializer.Deserialize<TResponse>(rawContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (fullResponse == null)
                        return (default, "Respuesta nula del servidor");

                    if (!fullResponse.Success)
                        return (default, $"{fullResponse.Message} (Código {fullResponse.Code})");

                    return (fullResponse, null); // Se devuelve todo el objeto, no solo .Data
                }
                catch (JsonException jex)
                {
                    _logger.LogError(jex, "Error al deserializar como {Type}", typeof(TResponse).Name);
                    return (default, "No se pudo deserializar la respuesta del servidor");
                }
            }

            // Error HTTP
            // 1. Si el body está vacío, devolver solo StatusCode
            if (string.IsNullOrWhiteSpace(rawContent))
            {
                return (
                    new { StatusCode = (int)response.StatusCode },
                    $"Error HTTP {(int)response.StatusCode} ({response.ReasonPhrase}) ({requestUri})"
                );
            }

            // 2. Si el body tiene contenido, intentar deserializarlo como TDataError
            try
            {
                var errorObject = JsonSerializer.Deserialize<TDataError>(rawContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return (
                    errorObject,
                    $"Error HTTP {(int)response.StatusCode} ({response.ReasonPhrase}) ({requestUri})"
                );
            }
            catch (JsonException jex)
            {
                _logger.LogError(jex, "Error al deserializar error como {Type}", typeof(TDataError).Name);
                return (
                    new { StatusCode = (int)response.StatusCode },
                    $"Error HTTP {(int)response.StatusCode} ({response.ReasonPhrase}) ({requestUri})"
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción inesperada durante GetAsync");
            return (default, $"Error inesperado: {ex.Message}");
        }
    }
}
