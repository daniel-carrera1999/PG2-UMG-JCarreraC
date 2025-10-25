using System.ComponentModel.DataAnnotations;

namespace AdoptaYA.Functionalities.RecoveryPassword.Model;
public class ResetPasswordRequestForm
{
    [Required(ErrorMessage = "Codigo requerido")]
    public int? EmployeeCode { get; set; }
}