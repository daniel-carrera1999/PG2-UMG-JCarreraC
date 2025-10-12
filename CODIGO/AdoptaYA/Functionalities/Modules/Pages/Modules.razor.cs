using AdoptaYA.Functionalities.Modules.Http;
using AdoptaYA.Functionalities.Modules.Model;
using AdoptaYA.Services.Dialogs;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdoptaYA.Functionalities.Modules.Pages;
public partial class Modules
{
    [Inject] ModulesHttp modulesHttp { get; set; } = default!;
    [Inject] CustomDialogService customDialogService { get; set; } = default!;

    bool loading = false;
    private IList<Modulo> modulos = new List<Modulo>();

    private bool isEdit = false;
    private Modulo newModulo = new Modulo();

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        modulos = await modulesHttp.GetAllModules();
        loading = false;

        StateHasChanged();
    }

    private async Task AddModulo()
    {
        bool run = await customDialogService.OpenConfirmAsync("¿Está a punto de crear un nuevo módulo, desea continuar?", "Confirmar", "Continuar", "Cancelar", new DialogOptions { CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false, ShowClose = false });

        if (run)
        {
            loading = true;
            bool response = await modulesHttp.CreateModule(newModulo);
            loading = false;

            if (response)
            {
                newModulo = new Modulo();
                await LoadDataAsync();
            }
        }
    }

    private async Task UpdateModulo()
    {
        bool run = await customDialogService.OpenConfirmAsync($"¿Está a punto de editar el módulo con id {newModulo.Id}, desea continuar?", "Confirmar", "Continuar", "Cancelar", new DialogOptions { CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false, ShowClose = false });

        if (run)
        {
            loading = true;
            bool response = await modulesHttp.UpdateModule(newModulo);
            loading = false;

            if (response)
            {
                newModulo = new Modulo();
                await LoadDataAsync();
            }
        }
    }

    private void CancelUpdate()
    {
        isEdit = false;
        newModulo = new Modulo();
        StateHasChanged();
    }

    private void SetUpdate(Modulo modulo)
    {
        isEdit = true;
        newModulo = new Modulo
        {
            Id = modulo.Id,
            Name = modulo.Name,
            Path = modulo.Path,
            Description = modulo.Description,
            Icon = modulo.Icon
        };
        StateHasChanged();
    }

    private bool IsFormIncomplete =>
        string.IsNullOrWhiteSpace(newModulo?.Name) ||
        string.IsNullOrWhiteSpace(newModulo?.Path) ||
        string.IsNullOrWhiteSpace(newModulo?.Description) ||
        string.IsNullOrWhiteSpace(newModulo?.Icon);

    private async Task DeleteModulo(int id)
    {
        bool run = await customDialogService.OpenConfirmAsync($"¿Está a punto de eliminar el módulo con id {id}, desea continuar?", "Confirmar", "Continuar", "Cancelar", new DialogOptions { CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false, ShowClose = false });

        if (run) 
        {
            loading = true;
            bool response = await modulesHttp.DeleteModule(id);
            loading = false;

            if (response)
            {
                newModulo = new Modulo();
                await LoadDataAsync();
            }

        }
    }

}