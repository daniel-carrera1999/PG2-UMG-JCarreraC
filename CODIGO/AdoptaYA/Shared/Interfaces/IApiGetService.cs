namespace AdoptaYA.Shared.Interfaces;
public interface IApiGetService
{
    Task<HttpResponseMessage> GetAsync(string url, string accept, int source = 1, bool log = false);
}
