using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Pets.Model;
public class MascotaResponseDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombre_mascota")]
    public string? Name { get; set; }

    [JsonPropertyName("tamanio")]
    public string? Size { get; set; }

    [JsonPropertyName("peso")]
    public int Weight { get; set; }

    [JsonPropertyName("color")]
    public string? color { get; set; }

    [JsonPropertyName("comportamiento")]
    public string? Behavior { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("inactive")]
    public int Inactive { get; set; }

    [JsonPropertyName("id_animal")]
    public int IdAnimal { get; set; }

    [JsonPropertyName("animal")]
    public AnimalResponseDTO? Animal { get; set; }

    [JsonPropertyName("vacunas")]
    public List<VacunaResponseDTO>? Vaccines { get; set; }

    [JsonPropertyName("enfermedads")]
    public List<EnfermedadResponseDTO>? Diseases { get; set; }
}