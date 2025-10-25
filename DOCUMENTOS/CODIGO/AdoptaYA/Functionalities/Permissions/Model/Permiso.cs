using AdoptaYA.Functionalities.Modules.Model;
using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Permissions.Model;

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

    [JsonPropertyName("modulo")]
    public Modulo? Module { get; set; }
}