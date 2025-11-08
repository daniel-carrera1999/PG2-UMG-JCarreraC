using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Pets.Model;
public class VacunaRequestDTO
{
    [JsonPropertyName("descripcion")]
    public string? Description { get; set; }

    [JsonPropertyName("aplicada")]
    public int? Applied { get; set; } = 0;

    [JsonPropertyName("fecha_aplicacion")]
    public DateOnly? DateApplied { get; set; }
}