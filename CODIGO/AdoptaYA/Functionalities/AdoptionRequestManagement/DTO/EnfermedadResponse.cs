using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
public class EnfermedadResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("descripcion")]
    public string? Description { get; set; }

    [JsonPropertyName("tratamiento")]
    public string? Treatment { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("medicinas")]
    public List<MedicinaResponse>? Medicines { get; set; }
}