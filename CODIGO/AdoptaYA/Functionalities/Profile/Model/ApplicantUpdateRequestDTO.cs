using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Profile.Model;
public class ApplicantUpdateRequestDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [JsonPropertyName("nombres")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [JsonPropertyName("apellidos")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "El número de identificación es obligatorio")]
    [JsonPropertyName("dpi")]
    public string? Dpi { get; set; }

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
    public string? MaritalStatus { get; set; }

    [JsonPropertyName("ocupacion")]
    public string? Occupation { get; set; }

    [Required(ErrorMessage = "La foto del solicitante es obligatoria")]
    [JsonPropertyName("foto")]
    public string? RequestPhoto { get; set; }

    [JsonPropertyName("id_usuario")]
    public int IdUser { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("inactive")]
    public int Inactive { get; set; }
}