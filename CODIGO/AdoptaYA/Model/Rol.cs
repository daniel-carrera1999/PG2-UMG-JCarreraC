using System.Text.Json.Serialization;

namespace AdoptaYA.Model;

public class Rol
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombre")]
    public string? Name { get; set; }

    [JsonPropertyName("descripcion")]
    public string? Description { get; set; }
}