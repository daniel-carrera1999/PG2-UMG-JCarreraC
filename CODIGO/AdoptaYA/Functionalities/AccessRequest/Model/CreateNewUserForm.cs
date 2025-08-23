using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AccessRequest.Model;
public class CreateNewUserForm
{
    [Required(ErrorMessage = "Codigo requerido")]
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Correo es requerido")]
    [JsonPropertyName("correo")]
    public string? Email {  get; set; }

    [Required(ErrorMessage = "Contraseña requerida")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
    ErrorMessage = "La contraseña debe tener al menos 8 caracteres, una letra mayúscula, una minúscula, un número y un carácter especial.")]
    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Repite tu contraseña")]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
    [JsonPropertyName("repeatPassword")]
    public string? RepeatPassword { get; set; }

    [Required(ErrorMessage = "Es requerido al menos un nombre")]
    [JsonPropertyName("nombre")]
    public string? name { get; set; }

    [Required(ErrorMessage = "Es requerido al menos un apellido")]
    [JsonPropertyName("apellido")]
    public string? lastname { get; set; }
}
