using AdoptaYA.Functionalities.Pets.Http;
using AdoptaYA.Functionalities.Pets.Model;
using Microsoft.AspNetCore.Components;

namespace AdoptaYA.Functionalities.Pets.Components;
public partial class ViewPet
{
    [Inject] PetsHttp petsHttp { get; set; } = default!;

    [Parameter] public int PetId { get; set; }

    private bool loading = false;
    private MascotaRequestDTO pet = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        pet = await petsHttp.GetPetAsync(PetId);
        loading = false;

        StateHasChanged();
    }

}