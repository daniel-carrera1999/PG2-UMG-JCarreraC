using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Log.Model;
public class Bitacora
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("tabla")]
    public string? Table { get; set; }

    [JsonPropertyName("accion")]
    public string? Action { get; set; }

    [JsonPropertyName("fecha")]
    public DateTime Date { get; set; }

    [JsonPropertyName("datos")]
    public string? Data { get; set; }

    [JsonPropertyName("id_usuario")]
    public int UserId { get; set; }

    [JsonPropertyName("usuario")]
    public BitacoraUsuario? User { get; set; }
}