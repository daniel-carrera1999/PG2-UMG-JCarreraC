using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Pets.Model;

public class MascotaRequestDTO
{
    [Required(ErrorMessage = "El nombre de la mascota es obligatorio")]
    [JsonPropertyName("nombre_mascota")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "El tamaño es obligatorio")]
    [JsonPropertyName("tamanio")]
    public string? Size { get; set; }

    [Required(ErrorMessage = "El peso es obligatorio")]
    [Range(0.1, 500, ErrorMessage = "El peso debe estar entre 0.1 y 500 kg")]
    [JsonPropertyName("peso")]
    public double? Weight { get; set; }

    [Required(ErrorMessage = "El color es obligatorio")]
    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [Required(ErrorMessage = "El comportamiento es obligatorio")]
    [JsonPropertyName("comportamiento")]
    public string? Behavior { get; set; }

    [Required(ErrorMessage = "La foto principal es obligatoria")]
    [JsonPropertyName("foto_principal")]
    public string? MainPhoto { get; set; }

    [JsonPropertyName("foto_secundaria")]
    public string? SecondaryPhoto { get; set; }

    [JsonPropertyName("foto_adicional")]
    public string? AdditionalPhoto { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un tipo de animal")]
    [JsonPropertyName("id_animal")]
    public int? IdAnimal { get; set; }

    [JsonPropertyName("vacunas")]
    public List<VacunaRequestDTO>? Vaccines { get; set; }

    [JsonPropertyName("enfermedades")]
    public List<EnfermedadRequestDTO>? Diseases { get; set; }
}