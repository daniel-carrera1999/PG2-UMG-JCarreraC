using AdoptaYA.Services.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;
using AdoptaYA.Functionalities.Authentication.Model.Session;

namespace AdoptaYA.Extensions;
public static class AuthExtension
{
    public static RouteGroupBuilder MapAuthRoutes(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/auth");

        group.MapGet("/start-session", async (HttpContext context, AuthSessionService sessionService) =>
        {
            var token = context.Request.Cookies["temp_token"];
            //var userEncoded = context.Request.Cookies["temp_user"];

            var userJsonBuilder = new System.Text.StringBuilder();
            int fragmentIndex = 0;
            while (true)
            {
                var key = $"temp_user_{fragmentIndex}";
                if (!context.Request.Cookies.ContainsKey(key)) break;
                userJsonBuilder.Append(System.Uri.UnescapeDataString(context.Request.Cookies[key]!));
                fragmentIndex++;
            }
            var userEncoded = userJsonBuilder.Length > 0 ? userJsonBuilder.ToString() : null;

            var refreshTokenTemp = context.Request.Cookies["temp_refresh_token"];
            var exp_refreshTokenTemp = context.Request.Cookies["temp_exp"];

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userEncoded))
            {
                return Results.Redirect("/");
            }

            var userJson = Uri.UnescapeDataString(userEncoded);
            var user = JsonSerializer.Deserialize<SessionUser>(userJson);
            var refreshToken = Uri.UnescapeDataString(refreshTokenTemp!);
            var expRefreshToken = Uri.UnescapeDataString(exp_refreshTokenTemp!);

            if (user == null)
            {
                return Results.Redirect("/");
            }

            await sessionService.CreateSessionAsync(token, user, refreshToken, expRefreshToken);

            context.Response.Cookies.Delete("temp_token");
            //context.Response.Cookies.Delete("temp_user");

            // Borrar los fragmentos temp_user_N
            for (int i = 0; i < fragmentIndex; i++)
            {
                context.Response.Cookies.Delete($"temp_user_{i}");
            }


            context.Response.Cookies.Delete("temp_refresh_token");
            context.Response.Cookies.Delete("temp_exp");

            return Results.Redirect("/home");
        });

        group.MapGet("/logout", async (HttpContext context) =>
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Redirect("/home");
        });

        group.MapGet("/logout-background", async (HttpContext context) =>
        {
            try
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.Ok();
            } catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });

        group.MapPost("/refresh-session", async (
            HttpContext context,
            AuthSessionService sessionService,
            RefreshSessionPayload payload) =>
        {
            if (payload is null || string.IsNullOrEmpty(payload.AccessToken) || payload.User is null)
                return Results.BadRequest("Invalid session data");

            try
            {
                await sessionService.CreateSessionAsync(
                    payload.AccessToken,
                    payload.User,
                    payload.RefreshToken,
                    payload.RefreshTokenExp
                );

                return Results.Ok();

            } catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });


        return group;
    }
}