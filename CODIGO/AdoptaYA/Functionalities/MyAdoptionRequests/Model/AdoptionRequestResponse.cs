using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.MyAdoptionRequests.Model;
public class AdoptionRequestResponse
{
    [JsonPropertyName("id_adopcion")]
    public int AdoptionId { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("nombre_mascota")]
    public string? Name { get; set; }

    [JsonPropertyName("raza")]
    public string? Breed { get; set; }

    [JsonPropertyName("especie")]
    public string? Species { get; set; }
}