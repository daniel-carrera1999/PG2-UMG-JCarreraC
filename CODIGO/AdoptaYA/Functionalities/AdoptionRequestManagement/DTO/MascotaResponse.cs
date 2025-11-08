using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
public class MascotaResponse
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
    public AnimalResponse? Animal { get; set; }

    [JsonPropertyName("enfermedades")]
    public List<EnfermedadResponse>? Diseases { get; set; }

    [JsonPropertyName("vacunas")]
    public List<VacunaResponse>? Vaccines { get; set; }
}