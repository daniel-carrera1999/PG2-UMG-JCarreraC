using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
public class ApplicantResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombres")]
    public string? Name { get; set; }

    [JsonPropertyName("apellidos")]
    public string? LastName { get; set; }

    [JsonPropertyName("fecha_nacimiento")]
    public DateTime? BirthDate { get; set; }

    [JsonPropertyName("celular")]
    public string? CellPhone { get; set; }

    [JsonPropertyName("telefono_casa")]
    public string? HomePhone { get; set; }

    [JsonPropertyName("correo")]
    public string? Email { get; set; }

    [JsonPropertyName("direccion")]
    public string? Address { get; set; }

    [JsonPropertyName("ingresos")]
    public int Income { get; set; }

    [JsonPropertyName("estado_civil")]
    public string? MaritalStatus { get; set; }

    [JsonPropertyName("ocupacion")]
    public string? Occupation { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("usuario")]
    public UsuarioResponse? User { get; set; }
}