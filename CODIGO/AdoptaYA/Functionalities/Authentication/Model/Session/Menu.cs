using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Authentication.Model.Session;
public class Menu
{
    [JsonPropertyName("Nombre")]
    public string? Name { get; set; }

    [JsonPropertyName("Path")]
    public string? Path { get; set; }

    [JsonPropertyName("Icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("Permisos")]
    public Permissions? Permissions { get; set; }
}