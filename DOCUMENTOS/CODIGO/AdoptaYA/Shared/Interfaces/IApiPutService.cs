namespace AdoptaYA.Shared.Interfaces;

public interface IApiPutService
{
    Task<HttpResponseMessage> PutAsync<TRequest>(string url, TRequest? data = default, int source = 1, bool log = false);
    Task<HttpResponseMessage> PutWithoutBodyRequestAsync(string url, int source = 1, bool log = false);
}