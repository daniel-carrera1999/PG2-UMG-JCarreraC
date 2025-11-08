using System.Text.Json.Serialization;
namespace AdoptaYA.Functionalities.AdoptPet.Model;
public class AvailableAnimal
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("especie")]
    public string? Species { get; set; }

    [JsonPropertyName("raza")]
    public string? Breed { get; set; }
}