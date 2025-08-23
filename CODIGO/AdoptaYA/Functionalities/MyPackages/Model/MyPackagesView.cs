using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AdoptaYA.Functionalities.MyPackages.Model;
public class MyPackagesView
{
    [JsonPropertyName("IdPaquete")]
    public int PackageId { get; set; }

    [JsonPropertyName("Remitente")]
    public string? Sender { get; set; }

    [JsonPropertyName("UbicacionRemitente")]
    public string? SenderLocation { get; set; }

    [JsonPropertyName("Destinatario")]
    public string? Addressee { get; set; }

    [JsonPropertyName("UbicacionDestinatario")]
    public string? DestinationLocation { get; set; }

    [JsonPropertyName("FechaCreacion")]
    public DateTime Created { get; set; }
    public string CreatedDate => Created.ToString("dd/MM/yyyy");

    [JsonPropertyName("IdEstado")]
    public int StateId { get; set; }

    [JsonPropertyName("Estado")]
    public string? State { get; set; }
}