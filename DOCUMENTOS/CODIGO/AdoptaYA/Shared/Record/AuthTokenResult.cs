namespace AdoptaYA.Shared.Record;
public record AuthTokenResult(string? AccessToken, string? exp, string? RefreshToken, string? expRefreshToken);