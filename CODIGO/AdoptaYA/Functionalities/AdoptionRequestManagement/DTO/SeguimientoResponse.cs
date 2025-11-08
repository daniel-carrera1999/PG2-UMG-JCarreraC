using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
public class SeguimientoResponse
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("fecha_seguimiento")]
	public DateTime? FollowUpDate { get; set; }

	[JsonPropertyName("observaciones")]
	public String? Observations { get; set; }

	[JsonPropertyName("date")]
	public DateTime? Date { get; set; }
}