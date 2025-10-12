using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Modules.Model;

public class Permiso
{
    [JsonPropertyName("id_rol")]
    public int RoleId { get; set; }

    [JsonPropertyName("id_modulo")]
    public int ModuleId { get; set; }

    [JsonPropertyName("create")]
    public int Create { get; set; }

    [JsonPropertyName("read")]
    public int Read { get; set; }

    [JsonPropertyName("update")]
    public int Update { get; set; }

    [JsonPropertyName("delete")]
    public int Delete { get; set; }

    [JsonPropertyName("rol")]
    public Rol? Role { get; set; }
}