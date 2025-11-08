using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.AdoptPet.Model;
public class AdoptionRequestDTO
{
    [JsonPropertyName("id_usuario")]
    public int UserId { get; set; }

    [JsonPropertyName("id_mascota")]
    public int PetId { get; set; }
}