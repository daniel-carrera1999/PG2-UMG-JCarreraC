namespace AdoptaYA.Shared.Interfaces;
public interface IApiPatchService
{
    Task<HttpResponseMessage> PatchAsync<TRequest>(string url, TRequest? data = default, int source = 1, bool log = false);
    Task<HttpResponseMessage> PatchWithoutBodyRequestAsync(string url, int source = 1, bool log = false);
}