using System.Text.Json.Serialization;

namespace AdoptaYA.Shared.Model;
public class Location
{
    [JsonPropertyName("IdUbicacion")]
    public int LocationId { get; set; }

    [JsonPropertyName("Codigo")]
    public string? LocationCode { get; set; }

    [JsonPropertyName("Nombre")]
    public string? LocationName { get; set; }

    [JsonPropertyName("Estado")]
    public bool Status { get; set; }
}