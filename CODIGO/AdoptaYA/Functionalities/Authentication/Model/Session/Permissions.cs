using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Authentication.Model.Session;
public class Permissions
{
    [JsonPropertyName("Create")]
    public bool Create { get; set; }

    [JsonPropertyName("Read")]
    public bool Read { get; set; }

    [JsonPropertyName("Update")]
    public bool Update { get; set; }

    [JsonPropertyName("Delete")]
    public bool Delete { get; set; }
}