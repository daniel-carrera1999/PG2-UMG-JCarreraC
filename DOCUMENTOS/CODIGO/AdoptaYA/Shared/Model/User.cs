using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdoptaYA.Shared.Model;
public class User
{
    [JsonPropertyName("IdUsuario")]
    public int Id { get; set; }

    [JsonPropertyName("Nombre")]
    public string? Name { get; set; }

    [JsonPropertyName("Correo")]
    public string? Email { get; set; }

    [JsonPropertyName("IdUbicacion")]
    public int? LocationId { get; set; }
}