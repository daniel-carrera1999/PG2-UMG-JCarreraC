using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Profile.Model;
public class PersonalReferenceResponseDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombre")]
    public string? Name { get; set; }

    [JsonPropertyName("telefono")]
    public string? CellPhone { get; set; }

    [JsonPropertyName("vinculo")]
    public string? Relationship { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("inactive")]
    public int Inactive { get; set; }

    [JsonPropertyName("id_solicitante")]
    public int IdApplicant { get; set; }
}