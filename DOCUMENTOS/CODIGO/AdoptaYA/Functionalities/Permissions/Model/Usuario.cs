using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Permissions.Model;
public class Usuario
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("correo")]
    public string? Email { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("nombre")]
    public string? FirstName { get; set; }

    [JsonPropertyName("apellido")]
    public string? LastName { get; set; }

    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }

    [JsonPropertyName("inactive")]
    public int Inactive { get; set; }
}