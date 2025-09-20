using System.Text.Json.Serialization;

namespace AdoptaYA.Model;

public class Modulo
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombre")]
    public string? Name { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("descripcion")]
    public string? Description { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("permisos")]
    public List<Permiso>? Permissions { get; set; }
}