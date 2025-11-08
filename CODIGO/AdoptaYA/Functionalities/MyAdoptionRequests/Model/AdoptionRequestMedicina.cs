using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.MyAdoptionRequests.Model;
public class AdoptionRequestMedicina
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombre")]
    public string? Name { get; set; }

    [JsonPropertyName("descripcion")]
    public string? Description { get; set; }

    [JsonPropertyName("indicaciones")]
    public string? Indications { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}