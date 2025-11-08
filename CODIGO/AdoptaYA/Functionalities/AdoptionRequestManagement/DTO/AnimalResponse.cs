using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
public class AnimalResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("especie")]
    public string? Species { get; set; }

    [JsonPropertyName("raza")]
    public string? Breed { get; set; }
}