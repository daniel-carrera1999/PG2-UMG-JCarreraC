namespace AdoptaYA.Shared.Interfaces;

public interface IApiDeleteService
{
    Task<HttpResponseMessage> DeleteAsync(string url, int id, int source = 1, bool log = false);
}