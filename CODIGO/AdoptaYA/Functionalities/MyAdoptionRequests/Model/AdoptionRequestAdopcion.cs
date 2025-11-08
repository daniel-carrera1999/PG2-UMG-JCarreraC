using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.MyAdoptionRequests.Model;
public class AdoptionRequestAdopcion
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("fecha_adopcion")]
    public DateTime AdoptionDate { get; set; }

    [JsonPropertyName("no_doc")]
    public int? DocumentNumber { get; set; }

    [JsonPropertyName("adjunto")]
    public string? Attachment { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}