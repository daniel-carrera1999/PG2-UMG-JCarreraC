using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Pets.Model;
public class EnfermedadRequestDTO
{
    [JsonPropertyName("descripcion")]
    public string? Description { get; set; }

    [JsonPropertyName("tratamiento")]
    public string? Treatment { get; set; }

    [JsonPropertyName("medicinas")]
    public List<MedicinaRequestDTO>? Medications { get; set; }
}
