using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
public class VacunaResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("descripcion")]
    public string? Description { get; set; }

    [JsonPropertyName("aplicada")]
    public int Applied { get; set; }

    [JsonPropertyName("fecha_aplicacion")]
    public DateTime ApplicationDate { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}