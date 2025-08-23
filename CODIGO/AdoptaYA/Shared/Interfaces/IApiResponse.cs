namespace AdoptaYA.Shared.Interfaces;
public interface IApiResponse<T>
{
    bool Success { get; }
    string Message { get; }
    int Code { get; }
    T? Data { get; }
}