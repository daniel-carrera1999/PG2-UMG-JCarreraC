using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Permissions.Model;
public class Rol
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombre")]
    public string? Name { get; set; }

    [JsonPropertyName("descripcion")]
    public string? Description { get; set; }

    [JsonPropertyName("permisos")]
    public List<Permiso>? Permissions { get; set; }

    [JsonPropertyName("usuarios")]
    public List<Usuario>? Users { get; set; }
}