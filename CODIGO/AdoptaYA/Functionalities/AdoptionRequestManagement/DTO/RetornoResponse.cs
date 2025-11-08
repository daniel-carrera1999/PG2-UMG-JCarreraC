using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
public class RetornoResponse
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("fecha_de_retorno")]
	public DateTime? ReturnDate { get; set; }

	[JsonPropertyName("observaciones")]
	public string? Observations { get; set; }

	[JsonPropertyName("date")]
	public DateTime? Date { get; set; }
}