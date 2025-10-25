using System.Threading.Tasks;
using AdoptaYA.Shared.Record;

namespace AdoptaYA.Shared.Interfaces;
public interface IAuthCookieService
{
    Task<AuthCookieResult?> GetAuthCookie(HttpResponseMessage response);
}