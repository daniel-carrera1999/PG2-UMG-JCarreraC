using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Log.Model;
public class BitacoraUsuario
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public string? UserName { get; set; }
}