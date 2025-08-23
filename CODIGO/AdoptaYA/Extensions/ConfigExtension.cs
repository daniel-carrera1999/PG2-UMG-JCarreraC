using AdoptaYA.Shared.Model;
using Microsoft.Extensions.Options;

namespace AdoptaYA.Extensions;
public static class ConfigExtension
{
    public static void AddConfigureClient(this IServiceCollection services, IConfiguration configuration)
    {
        var apiSettingsSection = configuration.GetSection("ApiSettings");

        // Opciones + acceso directo si lo necesitas en otros sitios
        services.Configure<ApiSettings>(apiSettingsSection);
        services.AddSingleton(apiSettingsSection.Get<ApiSettings>()!);

        services.AddScoped(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;

            HttpClientHandler handler;

#if DEBUG
            handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
#else
            handler = new HttpClientHandler();
#endif

            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromMinutes(settings.TimeoutMinutes)
            };

            client.DefaultRequestHeaders.UserAgent.ParseAdd("AdoptaYA/1.0");
            return client;
        });
    }
}
