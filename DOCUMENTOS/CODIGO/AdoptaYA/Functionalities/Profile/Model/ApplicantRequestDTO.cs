using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Profile.Model;
public class ApplicantRequestDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [JsonPropertyName("nombres")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [JsonPropertyName("apellidos")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    [JsonPropertyName("fecha_nacimiento")]
    public DateOnly? Birthdate { get; set; }

    [Required(ErrorMessage = "El celular es obligatorio")]
    [JsonPropertyName("celular")]
    public string? CellPhone { get; set; }

    [JsonPropertyName("telefono_casa")]
    public string? HomePhone { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [JsonPropertyName("correo")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "La dirección es obligatoria")]
    [JsonPropertyName("direccion")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Los ingresos mensuales son obligatorios")]
    [JsonPropertyName("ingresos")]
    public double? MonthlyIncome { get; set; }

    [JsonPropertyName("estado_civil")]
    public string? MaritalState { get; set; }

    [JsonPropertyName("ocupacion")]
    public string? Occupation { get; set; }

    [JsonPropertyName("id_usuario")]
    public int IdUsuario { get; set; }

    [JsonPropertyName("inactive")]
    public int Inactive { get; set; }
}
