using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Profile.Model;
public class ApplicantResponseDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombres")]
    public string? Name { get; set; }

    [JsonPropertyName("apellidos")]
    public string? LastName { get; set; }

    [JsonPropertyName("dpi")]
    public string? Dpi { get; set; }

    [JsonPropertyName("fecha_nacimiento")]
    public DateOnly Birthdate { get; set; }

    [JsonPropertyName("celular")]
    public string? CellNumber { get; set; }

    [JsonPropertyName("telefono_casa")]
    public string? HomePhone { get; set; }

    [JsonPropertyName("correo")]
    public string? Email { get; set; }

    [JsonPropertyName("direccion")]
    public string? Address { get; set; }

    [JsonPropertyName("ingresos")]
    public double MonthlyIncome { get; set; }

    [JsonPropertyName("estado_civil")]
    public string? MaritalStatus { get; set; }

    [JsonPropertyName("ocupacion")]
    public string? Occupation { get; set; }

    [JsonPropertyName("foto")]
    public string? RequestPhoto { get; set; }

    [JsonPropertyName("id_usuario")]
    public int IdUser { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("inactive")]
    public int Inactive { get; set; }
}