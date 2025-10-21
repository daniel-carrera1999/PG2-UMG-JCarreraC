using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Animals.Model;
public class Animal
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("especie")]
    public string? Species { get; set; }

    [JsonPropertyName("raza")]
    public string? Breed { get; set; }

    [JsonPropertyName("inactive")]
    public int Inactive { get; set; }

    public bool IsInactive => Inactive == 0 ? false : true;

    public string DisplayName => $"{Species} - {Breed}";

}