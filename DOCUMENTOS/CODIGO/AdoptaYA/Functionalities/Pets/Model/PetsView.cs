using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Pets.Model;

public class PetsView
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombre_mascota")]
    public string? Name { get; set; }

    [JsonPropertyName("tamanio")]
    public string? Size { get; set; }

    [JsonPropertyName("foto_principal")]
    public string? PrincipalPhoto { get; set; }

    [JsonPropertyName("peso")]
    public int Weight { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("comportamiento")]
    public string? Behavior { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("id_animal")]
    public int IdAnimal { get; set; }

    [JsonPropertyName("animal")]
    public PetsAnimalView? Animal { get; set; }
}

public class PetsAnimalView
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("especie")]
    public string? Species { get; set; }

    [JsonPropertyName("raza")]
    public string? Breed { get; set; }
}