using System.Text.Json.Serialization;
namespace AdoptaYA.Functionalities.AdoptPet.Model;
public class AvailablePet
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombre_mascota")]
    public string? PetName { get; set; }

    [JsonPropertyName("tamanio")]
    public string? Size { get; set; }

    [JsonPropertyName("foto_principal")]
    public string? MainPhoto { get; set; }

    [JsonPropertyName("peso")]
    public int Weight { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("comportamiento")]
    public string? Behavior { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("id_animal")]
    public int AnimalId { get; set; }

    [JsonPropertyName("animal")]
    public AvailableAnimal? Animal { get; set; }

}