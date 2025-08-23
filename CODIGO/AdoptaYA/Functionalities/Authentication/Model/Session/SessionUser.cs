using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Authentication.Model.Session;
public class SessionUser
{
    [JsonPropertyName("Username")]
    public required string Username { get; set; }

    [JsonPropertyName("Nombre")]
    public required string Name { get; set; }

    [JsonPropertyName("Correo")]
    public required string Email { get; set; }

    [JsonPropertyName("Rol")]
    public required string Rol { get; set; }

    [JsonPropertyName("Menu")]
    public List<Menu>? Menu { get; set; }
}