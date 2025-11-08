using System.Text.Json;
using AdoptaYA.Functionalities.Pets.Http;
using AdoptaYA.Functionalities.Pets.Model;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using AdoptaYA.Functionalities.Animals.Model;
using AdoptaYA.Services.Security;
using AdoptaYA.Functionalities.AdoptPet.Model;
using AdoptaYA.Functionalities.AdoptPet.Http;

namespace AdoptaYA.Functionalities.AdoptPet.Components;
public partial class ViewPet
{
    [Inject] PetsHttp petsHttp { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;
    [Inject] GetCurrentUser GetCurrentUser { get; set; } = default!;
    [Inject] AdoptPetHttp AdoptPetHttp { get; set; } = default!;
    [Inject] NotificationService NotificationService { get; set; } = default!;
    [Parameter] public int PetId { get; set; }

    private bool loading = false;
    private MascotaResponseDTO pet = new();
    private PetPhotosView photos = new();
    private List<string> size = new List<string>();
    private IList<Animal> animals = new List<Animal>();
    private AdoptionRequestDTO requestDTO = new();

    private RadzenDataGrid<EnfermedadResponseDTO>? diseasesGrid;

    void RowRenderDiseases(RowRenderEventArgs<EnfermedadResponseDTO> args)
    {
        args.Expandable = args.Data.Medications != null && args.Data.Medications.Any();
    }

    protected override async Task OnInitializedAsync()
    {
        size.Add("Pequeño");
        size.Add("Mediano");
        size.Add("Grande");

        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;

        // Cargar animales disponibles
        animals = await petsHttp.GetAllAnimals();

        // Cargar datos de la mascota
        pet = await petsHttp.GetPetAsync(PetId);

        // Cargar fotos
        photos = await petsHttp.GetPetPhotos(PetId, false);

        loading = false;
        StateHasChanged();
    }

    private async Task AdoptPet()
    {
        loading = true;
        var user = await GetCurrentUser.GetUserInfoAsync();
        requestDTO.UserId = user.UserId ?? 0;
        requestDTO.PetId = pet.Id;

        bool res = await AdoptPetHttp.CreateAdoption(requestDTO);

        loading = false;

        if (res) 
        {
            NotificationService.Notify(
                new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Solicitud de adopción enviada",
                    Detail = "Tu solicitud ha sido recibida. Te contactaremos pronto para continuar con el proceso",
                    Duration = 5000
                }
            );
            DialogService.Close(true);
        }

    }

    void Close()
    {
        DialogService.Close(false);
    }
}