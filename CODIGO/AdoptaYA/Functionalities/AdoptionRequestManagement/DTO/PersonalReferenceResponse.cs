using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AdoptionRequestManagement.DTO;
public class PersonalReferenceResponse
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

}