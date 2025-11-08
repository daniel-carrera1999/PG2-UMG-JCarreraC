using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.MyAdoptionRequests.Model;
public class AdoptionRequestMascota
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombre_mascota")]
    public string? PetName { get; set; }

    [JsonPropertyName("tamanio")]
    public string? Size { get; set; }

    [JsonPropertyName("peso")]
    public int Weight { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("comportamiento")]
    public string? Behavior { get; set; }

    [JsonPropertyName("foto_principal")]
    public string? MainPhoto { get; set; }

    [JsonPropertyName("foto_secundaria")]
    public string? SecondaryPhoto { get; set; }

    [JsonPropertyName("foto_adicional")]
    public string? AdditionalPhoto { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("animal")]
    public AdoptionRequestAnimal? Animal { get; set; }

    [JsonPropertyName("enfermedades")]
    public List<AdoptionRequestEnfermedad>? Diseases { get; set; }

    [JsonPropertyName("vacunas")]
    public List<AdoptionRequestVacuna>? Vaccines { get; set; }
}