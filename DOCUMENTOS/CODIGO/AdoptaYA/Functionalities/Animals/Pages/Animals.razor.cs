using AdoptaYA.Functionalities.Animals.Http;
using AdoptaYA.Functionalities.Animals.Model;
using AdoptaYA.Functionalities.Modules.Http;
using AdoptaYA.Services.Dialogs;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdoptaYA.Functionalities.Animals.Pages;
public partial class Animals
{
    [Inject] AnimalHttp AnimalHttp { get; set; } = default!;
    [Inject] CustomDialogService CustomDialogService { get; set; } = default!;

    private bool loading = false;

    private IList<Animal> animals = new List<Animal>();
    private Animal newAnimal = new();
    private bool isEdit = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        animals = await AnimalHttp.GetAllAnimals();
        loading = false;

        StateHasChanged();
    }

    private void SetUpdate(Animal item)
    {
        isEdit = true;
        newAnimal = new Animal
        {
            Id = item.Id,
            Species = item.Species,
            Breed = item.Breed,
            Inactive = item.Inactive
        };
        StateHasChanged();
    }

    private async Task AddAnimalAsync()
    {
        bool run = await CustomDialogService.OpenConfirmAsync("¿Está a punto de crear un nuevo animal, desea continuar?", "Confirmar", "Continuar", "Cancelar", new DialogOptions { CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false, ShowClose = false });

        if (run)
        {
            loading = true;
            bool response = await AnimalHttp.CreateAnimal(newAnimal);
            loading = false;

            if (response)
            {
                newAnimal = new Animal();
                await LoadDataAsync();
            }
        }
    }

    private async Task UpdateAnimal()
    {
        bool run = await CustomDialogService.OpenConfirmAsync($"¿Está a punto de editar el animal con id {newAnimal.Id}, desea continuar?", "Confirmar", "Continuar", "Cancelar", new DialogOptions { CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false, ShowClose = false });

        if (run)
        {
            loading = true;
            bool response = await AnimalHttp.UpdateAnimal(newAnimal);
            loading = false;

            if (response)
            {
                newAnimal = new Animal();
                await LoadDataAsync();
            }
        }

        isEdit = false;
    }

    private void CancelUpdate()
    {
        isEdit = false;
        newAnimal = new Animal();
        StateHasChanged();
    }

    private bool IsFormIncomplete =>
        string.IsNullOrWhiteSpace(newAnimal?.Species) ||
        string.IsNullOrWhiteSpace(newAnimal?.Breed);

    private async Task DeleteAnimal(Animal item)
    {
        bool run = await CustomDialogService.OpenConfirmAsync($"¿Está a punto de eliminar el animal con id {item.Id}, desea continuar?", "Confirmar", "Continuar", "Cancelar", new DialogOptions { CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false, ShowClose = false });

        if (run)
        {
            loading = true;
            item.Inactive = 1;
            bool response = await AnimalHttp.UpdateAnimal(item);
            loading = false;

            if (response)
            {
                newAnimal = new Animal();
                await LoadDataAsync();
            }

        }
    }

}