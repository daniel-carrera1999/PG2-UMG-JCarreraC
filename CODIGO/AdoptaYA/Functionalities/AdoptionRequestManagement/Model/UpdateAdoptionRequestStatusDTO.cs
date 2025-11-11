using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.Model;
public class UpdateAdoptionRequestStatusDTO
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("id_usuario")]
    public int UserId { get; set; }
}