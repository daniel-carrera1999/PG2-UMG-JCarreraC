using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Pets.Model;
public class EnfermedadResponseDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("descripcion")]
    public string? Description { get; set; }

    [JsonPropertyName("tratamiento")]
    public string? Treatment { get; set; }

    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }

    [JsonPropertyName("inactive")]
    public int Inactive { get; set; }

    [JsonPropertyName("id_mascota")]
    public int IdMascota { get; set; }

    [JsonPropertyName("medicinas")]
    public List<MedicinaResponseDTO>? Medications { get; set; }
}