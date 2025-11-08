using AdoptaYA.Functionalities.Log.Pages;
using System.Text;
using AdoptaYA.Functionalities.Permissions.Components;
using AdoptaYA.Functionalities.Pets.Http;
using AdoptaYA.Functionalities.Pets.Model;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.JSInterop;

namespace AdoptaYA.Functionalities.Pets.Components;
public partial class PetsList
{
    [Inject] PetsHttp petsHttp { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;
    [Inject] IJSRuntime JS { get; set; } = default!;

    private bool loading = false;

    private IList<PetsView> pets = new List<PetsView>();

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    public async Task LoadDataAsync()
    {
        loading = true;
        pets = await petsHttp.GetAllPets();
        loading = false;

        StateHasChanged();
    }

    private async Task OpenViewPet(int id)
    {
        var response = await DialogService.OpenAsync<ViewPet>(
            $"Mascota ID: <strong>{id}</strong>",
            new Dictionary<string, object?>
            {
                { "PetId", id }
            },
            new DialogOptions { Width = "75%", ShowClose=false, CloseDialogOnEsc=false, CloseDialogOnOverlayClick=false }
        );
    }

    private async Task ExportToCsv()
    {
        if (pets == null || !pets.Any())
            return;

        var csv = new StringBuilder();
        csv.AppendLine("Id,Nombre,Especie,Raza,Color,Tamaño,Peso,Comportamiento");

        foreach (var pet in pets)
        {
            csv.AppendLine($"{pet.Id},\"{pet.Name}\",\"{pet.Animal!.Species}\",\"{pet.Animal!.Breed}\",\"{pet.Color}\",\"{pet.Size}\",\"{pet.Weight}\",\"{pet.Behavior}\"");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        var base64 = Convert.ToBase64String(bytes);
        var fileName = $"Mascotas registradas_{DateTime.Now:yyyyMMddHHmmss}.csv";

        await JS.InvokeVoidAsync("downloadFileFromBase64", fileName, "text/csv", base64);
    }

}