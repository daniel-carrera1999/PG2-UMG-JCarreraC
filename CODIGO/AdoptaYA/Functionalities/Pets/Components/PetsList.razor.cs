using AdoptaYA.Functionalities.Permissions.Components;
using AdoptaYA.Functionalities.Pets.Http;
using AdoptaYA.Functionalities.Pets.Model;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdoptaYA.Functionalities.Pets.Components;
public partial class PetsList
{
    [Inject] PetsHttp petsHttp { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;

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

    private async Task<string> GetPrincipalPhotoById(int id)
    {
        loading = true;
        PetPhotosView photos = await petsHttp.GetPrincipalPhoto(id);
        loading = false;

        return photos.principal_photo!;
    }

    private async Task OpenViewPet(int id)
    {
        var response = await DialogService.OpenAsync<ViewPet>(
            $"Mascota ID: <strong>{id}</strong>",
            new Dictionary<string, object?>
            {
                { "PetId", id }
            },
            new DialogOptions { Width = "75%" }
        );
    }

}