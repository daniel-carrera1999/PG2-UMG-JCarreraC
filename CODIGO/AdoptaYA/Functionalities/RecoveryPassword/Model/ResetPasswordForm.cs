using System.ComponentModel.DataAnnotations;

namespace AdoptaYA.Functionalities.RecoveryPassword.Model;
public class ResetPasswordForm
{
    [Required(ErrorMessage = "Codigo requerido")]
    public int? EmployeeCode { get; set; }

    [Required(ErrorMessage = "Contraseña requerida")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Contraseña requerida")]
    public string? RepeatPassword { get; set; }

    public bool samePasswords()
    {
        return Password == RepeatPassword;
    }
}