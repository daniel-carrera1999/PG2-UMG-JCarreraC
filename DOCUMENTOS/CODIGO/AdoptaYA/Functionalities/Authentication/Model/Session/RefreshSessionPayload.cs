namespace AdoptaYA.Functionalities.Authentication.Model.Session;
public class RefreshSessionPayload
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public string RefreshTokenExp { get; set; } = default!;
    public SessionUser User { get; set; } = default!;
}