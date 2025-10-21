using System.Text.Json.Serialization;

namespace AdoptaYA.Functionalities.Pets.Model;
public class PetPhotosView
{
    [JsonPropertyName("foto_principal")]
    public string? principal_photo { get; set; }

    [JsonPropertyName("foto_secundaria")]
    public string? secundary_photo { get; set; }

    [JsonPropertyName("foto_adicional")]
    public string? additional_photo { get; set; }
}