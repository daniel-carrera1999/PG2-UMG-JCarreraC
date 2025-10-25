using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Pets.Model;
public class MedicinaResponseDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombre")]
    public string? Name { get; set; }

    [JsonPropertyName("descripcion")]
    public string? Description { get; set; }

    [JsonPropertyName("indicaciones")]
    public string? Indications { get; set; }

    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }

    [JsonPropertyName("inactive")]
    public int Inactive { get; set; }

    [JsonPropertyName("id_enfermedad")]
    public int IdEnfermedad { get; set; }
}