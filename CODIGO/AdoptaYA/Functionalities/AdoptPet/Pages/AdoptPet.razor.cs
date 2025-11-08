using AdoptaYA.Functionalities.AdoptPet.Components;
using AdoptaYA.Functionalities.AdoptPet.Http;
using AdoptaYA.Functionalities.AdoptPet.Model;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdoptaYA.Functionalities.AdoptPet.Pages;

public partial class AdoptPet
{
    [Inject] AdoptPetHttp AdoptPetHttp { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;
 
    private bool loading = false;

    private IList<AvailablePet>? availablePets;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        availablePets = await AdoptPetHttp.GetAllAvailablePets();
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
            new DialogOptions { Width = "75%", ShowClose = false, CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false }
        );

        if (response == true)
        {
            await LoadDataAsync();
        }
    }

}