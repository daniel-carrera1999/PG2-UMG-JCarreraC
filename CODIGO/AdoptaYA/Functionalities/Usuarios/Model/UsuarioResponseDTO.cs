using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Usuarios.Model;
public class UsuarioResponseDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("correo")]
    public string? Email { get; set; }

    [JsonPropertyName("nombre")]
    public string? Name { get; set; }

    [JsonPropertyName("apellido")]
    public string? LastName { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("inactive")]
    public int Inactive { get; set; }

    [JsonPropertyName("rol")]
    public RolResponseDTO? Rol { get; set; }
}