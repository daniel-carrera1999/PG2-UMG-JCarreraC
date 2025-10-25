using AdoptaYA.Services.Http;
using AdoptaYA.Services.Security;
using AdoptaYA.Services.UI;
using AdoptaYA.Services.Dialogs;
using AdoptaYA.Shared.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using AdoptaYA.Services.SGL;
using AdoptaYA.Services.SGU;
using AdoptaYA.Functionalities.MyPackages.Http;
using AdoptaYA.Functionalities.Modules.Http;
using AdoptaYA.Functionalities.Permissions.Http;
using AdoptaYA.Functionalities.Usuarios.Http;
using AdoptaYA.Functionalities.Animals.Http;
using AdoptaYA.Functionalities.Pets.Http;
using AdoptaYA.Functionalities.Profile.Http;

namespace AdoptaYA.Extensions;
public static class ServiceExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<MainLayoutService>();
        services.AddScoped<ApiService>();
        services.AddScoped<ApiGetService>();
        services.AddScoped<ApiPostService>();
        services.AddScoped<ApiHeadService>();
        services.AddScoped<ApiPatchService>();
        services.AddScoped<ApiDeleteService>();
        services.AddScoped<ApiPutService>();
        services.AddScoped<SGLService>();
        services.AddScoped<SGUService>();
        services.AddScoped<MyPackagesHttp>();
        services.AddScoped<ModulesHttp>();
        services.AddScoped<RolesAndPermissionHttp>();
        services.AddScoped<UsuariosHttp>();
        services.AddScoped<AnimalHttp>();
        services.AddScoped<PetsHttp>();
        services.AddScoped<ProfileHttp>();

        services.AddScoped<IAuthCookieService, AuthCookieService>();
        services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        services.AddScoped<CustomAuthenticationStateProvider>();
        services.AddScoped<AccessControlService>();
        services.AddScoped<ISessionReasonService, SessionReasonService>();
        services.AddScoped<EncryptionService>();
        services.AddScoped<GetCurrentUser>();
        services.AddScoped<CustomDialogService>();

        services.AddHttpContextAccessor();
        services.AddScoped<AuthSessionService>();

        // Autenticación por cookies
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/";
                options.Cookie.Name = "GC.Auth";
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            });
        services.AddAuthorization();
    }

}