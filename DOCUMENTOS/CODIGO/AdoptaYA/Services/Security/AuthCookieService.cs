using AdoptaYA.Shared.Interfaces;
using AdoptaYA.Shared.Record;
using Microsoft.JSInterop;

namespace AdoptaYA.Services.Security;
public class AuthCookieService : IAuthCookieService
{
    // Get AuthCookie of Http Response
    public async Task<AuthCookieResult?> GetAuthCookie(HttpResponseMessage response)
    {
        if (response.Headers.TryGetValues("Set-Cookie", out var setCookieHeaders))
        {
            foreach (var cookieHeader in setCookieHeaders)
            {
                var segments = cookieHeader.Split(';', StringSplitOptions.RemoveEmptyEntries);

                var tokenPart = segments.FirstOrDefault(s => s.Trim().StartsWith("refresh_token="));
                var expiresPart = segments.FirstOrDefault(s => s.Trim().StartsWith("expires=", StringComparison.OrdinalIgnoreCase));

                if (tokenPart != null)
                {
                    var token = tokenPart.Split('=', 2)[1].Trim();

                    DateTime? expires = null;
                    if (expiresPart != null)
                    {
                        var expiresRaw = expiresPart.Split('=', 2)[1].Trim();
                        if (DateTime.TryParseExact(
                            expiresRaw,
                            "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.AdjustToUniversal,
                            out var parsedExpires))
                        {
                            expires = parsedExpires;
                        }
                    }

                    return new AuthCookieResult(token, expires);
                }
            }
        }

        return null;
    }
}