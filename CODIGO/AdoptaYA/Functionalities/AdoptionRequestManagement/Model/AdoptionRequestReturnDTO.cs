using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.Model;
public class AdoptionRequestReturnDTO
{
    [Required(ErrorMessage = "El campo fecha_de_retorno es requerido")]
    [JsonPropertyName("fecha_de_retorno")]
    public DateTime ReturnDate { get; set; }

    [Required(ErrorMessage = "El campo observaciones es requerido")]
    [JsonPropertyName("observaciones")]
    public string Observations { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo id_adopcion es requerido")]
    [JsonPropertyName("id_adopcion")]
    public int AdoptionId { get; set; }

    [Required(ErrorMessage = "El campo id_usuario es requerido")]
    [JsonPropertyName("id_usuario")]
    public int UserId { get; set; }
}