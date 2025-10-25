using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Pets.Model;
public class MedicinaRequestDTO
{
    [JsonPropertyName("nombre")]
    public string? Name { get; set; }

    [JsonPropertyName("descripcion")]
    public string? Description { get; set; }

    [JsonPropertyName("indicaciones")]
    public string? Indications { get; set; }
}