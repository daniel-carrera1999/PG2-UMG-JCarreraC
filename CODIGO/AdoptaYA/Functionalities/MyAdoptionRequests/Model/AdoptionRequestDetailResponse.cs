using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.MyAdoptionRequests.Model;
public class AdoptionRequestDetailResponse
{
    [JsonPropertyName("adopcion")]
    public AdoptionRequestAdopcion? Adoption { get; set; }

    [JsonPropertyName("solicitante")]
    public AdoptionRequestApplicant? Applicant { get; set; }

    [JsonPropertyName("mascota")]
    public AdoptionRequestMascota? Pet { get; set; }
}