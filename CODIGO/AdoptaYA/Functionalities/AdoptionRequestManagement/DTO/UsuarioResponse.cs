using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
public class UsuarioResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("correo")]
    public string? Email { get; set; }

    [JsonPropertyName("nombre")]
    public string? FirstName { get; set; }

    [JsonPropertyName("apellido")]
    public string? LastName { get; set; }
}