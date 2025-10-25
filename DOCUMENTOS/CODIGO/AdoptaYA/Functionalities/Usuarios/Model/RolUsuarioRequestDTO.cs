using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Usuarios.Model;
public class RolUsuarioRequestDTO
{
    [JsonPropertyName("id_rol")]
    public int RolId { get; set; }

    [JsonPropertyName("id_usuario")]
    public int UserId { get; set; }
}