using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
public class AdoptionRootResponse
{
    [JsonPropertyName("adopcion")]
    public AdoptionResponse? Adoption { get; set; }

    [JsonPropertyName("solicitante")]
    public ApplicantResponse? Applicant { get; set; }

    [JsonPropertyName("referencias_personales")]
    public List<PersonalReferenceResponse>? PersonalReferences { get; set; }

    [JsonPropertyName("mascota")]
    public MascotaResponse? Pet { get; set; }

    [JsonPropertyName("seguimientos")]
    public List<SeguimientoResponse>? FollowUp { get; set; }

    [JsonPropertyName("retornos")]
    public List<RetornoResponse>? ReturnPet { get; set; }
}