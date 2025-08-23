namespace AdoptaYA.Shared.Interfaces;
public interface IApiPostService
{
    Task<HttpResponseMessage> PostAsync<TRequest>(string url, TRequest? data = default, int source = 1, bool log = false);
}
