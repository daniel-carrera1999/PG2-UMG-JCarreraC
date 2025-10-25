namespace AdoptaYA.Shared.Model;
public class ApiSettings
{
    public string CoreApiUrl { get; set; } = default!;
    public string UniUserAuthApiUrl { get; set; } = default!;
    public string UniUserClientApiUrl { get; set; } = default!;
    public string UbicacionesApiUrl { get; set; } = default!;
    public int TimeoutMinutes { get; set; }
}