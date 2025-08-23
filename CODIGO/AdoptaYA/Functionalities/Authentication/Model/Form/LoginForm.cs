using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Authentication.Model.Form;
public class LoginForm
{
    [Required(ErrorMessage = "Correo requerido")]
    [JsonPropertyName("correo")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Contraseña requerida")]
    [JsonPropertyName("password")]
    public string? Password { get; set; }
}
