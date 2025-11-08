using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.MyAdoptionRequests.Model;
public class AdoptionRequestAnimal
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("especie")]
    public string? Species { get; set; }

    [JsonPropertyName("raza")]
    public string? Breed { get; set; }
}