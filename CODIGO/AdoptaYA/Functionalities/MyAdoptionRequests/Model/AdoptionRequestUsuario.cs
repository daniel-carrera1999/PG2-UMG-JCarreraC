using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.MyAdoptionRequests.Model;
public class AdoptionRequestUsuario
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