using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Pets.Model;
public class AnimalResponseDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("especie")]
    public string? Species { get; set; }

    [JsonPropertyName("raza")]
    public string? Breed { get; set; }

    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }

    [JsonPropertyName("inactive")]
    public int Inactive { get; set; }
}