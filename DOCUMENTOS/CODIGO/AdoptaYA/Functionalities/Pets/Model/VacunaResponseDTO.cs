using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Pets.Model;
public class VacunaResponseDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("descripcion")]
    public string? Description { get; set; }

    [JsonPropertyName("aplicada")]
    public int Applied { get; set; }

    [JsonPropertyName("fecha_aplicacion")]
    public DateOnly? DateApplied { get; set; }

    [JsonPropertyName("name")]
    public DateTime? Date { get; set; }

    [JsonPropertyName("inactive")]
    public int Inactive { get; set; }

    [JsonPropertyName("id_mascota")]
    public int IdMascota { get; set; }
}