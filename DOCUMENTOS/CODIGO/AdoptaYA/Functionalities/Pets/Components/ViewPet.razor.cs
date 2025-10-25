using System.Text.Json;
using AdoptaYA.Functionalities.Animals.Model;
using AdoptaYA.Functionalities.Pets.Http;
using AdoptaYA.Functionalities.Pets.Model;
using AdoptaYA.Services.Dialogs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using Radzen.Blazor;

namespace AdoptaYA.Functionalities.Pets.Components;

public partial class ViewPet
{
    [Inject] PetsHttp petsHttp { get; set; } = default!;
    [Inject] NotificationService NotificationService { get; set; } = default!;
    [Inject] CustomDialogService CustomDialogService { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;

    [Inject] ILogger<ViewPet> logger { get; set; } = default!;

    [Parameter] public int PetId { get; set; }
    [Parameter] public EventCallback<int> OnSave { get; set; }

    private bool loading = false;
    private bool OnlyRead = true;
    private MascotaResponseDTO pet = new();
    private MascotaRequestDTO form = new();
    private PetPhotosView photos = new();
    private List<string> size = new List<string>();
    private IList<Animal> animals = new List<Animal>();

    private RadzenDataGrid<EnfermedadResponseDTO> diseasesGrid;

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
        
        // Mapear los datos de Response a Request para el formulario
        MapPetToForm();


        logger.LogWarning(
        JsonSerializer.Serialize(pet, new JsonSerializerOptions { WriteIndented = true }));
        
        loading = false;
        StateHasChanged();
    }

    private void MapPetToForm()
    {
        form = new MascotaRequestDTO
        {
            Name = pet.Name,
            Size = pet.Size,
            Weight = pet.Weight,
            Color = pet.color,
            Behavior = pet.Behavior,
            IdAnimal = pet.IdAnimal,
            MainPhoto = photos.principal_photo,
            SecondaryPhoto = photos.secundary_photo,
            AdditionalPhoto = photos.additional_photo,
            Vaccines = pet.Vaccines?.Select(v => new VacunaRequestDTO
            {
                Description = v.Description,
                Applied = v.Applied,
                DateApplied = v.DateApplied
            }).ToList() ?? new List<VacunaRequestDTO>(),
            Diseases = pet.Diseases?.Select(d => new EnfermedadRequestDTO
            {
                Description = d.Description,
                Treatment = d.Treatment,
                Medications = d.Medications?.Select(m => new MedicinaRequestDTO
                {
                    Name = m.Name,
                    Description = m.Description,
                    Indications = m.Indications
                }).ToList() ?? new List<MedicinaRequestDTO>()
            }).ToList() ?? new List<EnfermedadRequestDTO>()
        };
    }

    void AddVaccine()
    {
        form.Vaccines ??= new List<VacunaRequestDTO>();
        form.Vaccines.Add(new VacunaRequestDTO());
        StateHasChanged();
    }

    void RemoveVaccine(int index)
    {
        if (form.Vaccines != null && index >= 0 && index < form.Vaccines.Count)
        {
            form.Vaccines.RemoveAt(index);
        }
    }

    void AddDisease()
    {
        form.Diseases ??= new List<EnfermedadRequestDTO>();
        form.Diseases.Add(new EnfermedadRequestDTO { Medications = new List<MedicinaRequestDTO>() });
    }

    void RemoveDisease(int index)
    {
        if (form.Diseases != null && index >= 0 && index < form.Diseases.Count)
        {
            form.Diseases.RemoveAt(index);
        }
    }

    void AddMedication(int diseaseIndex)
    {
        if (form.Diseases != null && diseaseIndex >= 0 && diseaseIndex < form.Diseases.Count)
        {
            form.Diseases[diseaseIndex].Medications ??= new List<MedicinaRequestDTO>();
            form.Diseases[diseaseIndex].Medications.Add(new MedicinaRequestDTO());
        }
    }

    void RemoveMedication(int diseaseIndex, int medIndex)
    {
        if (form.Diseases != null && diseaseIndex >= 0 && diseaseIndex < form.Diseases.Count)
        {
            var medications = form.Diseases[diseaseIndex].Medications;
            if (medications != null && medIndex >= 0 && medIndex < medications.Count)
            {
                medications.RemoveAt(medIndex);
            }
        }
    }

    private async Task OnFileChange(string e, string photoType)
    {
        dynamic eventArgs = e;
        if (eventArgs?.Files == null || eventArgs.Files.Count == 0)
            return;

        var file = (IBrowserFile)eventArgs.Files[0];
        const long maxFileSize = 10 * 1024 * 1024; // 10MB

        if (file.Size > maxFileSize)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Archivo muy grande",
                Detail = "La imagen no puede pesar más de 10 MB",
                Duration = 4000
            });
            return;
        }

        try
        {
            using var memoryStream = new MemoryStream();
            await file.OpenReadStream(maxAllowedSize: maxFileSize).CopyToAsync(memoryStream);
            var base64 = Convert.ToBase64String(memoryStream.ToArray());
            var dataUrl = $"data:{file.ContentType};base64,{base64}";

            switch (photoType)
            {
                case "main":
                    form.MainPhoto = dataUrl;
                    break;
                case "secondary":
                    form.SecondaryPhoto = dataUrl;
                    break;
                case "additional":
                    form.AdditionalPhoto = dataUrl;
                    break;
            }

            StateHasChanged();
        }
        catch (Exception ex)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Error",
                Detail = $"No se pudo cargar la imagen: {ex.Message}",
                Duration = 4000
            });
        }
    }

    private async Task SubmitAsync()
    {
        loading = true;

        await petsHttp.UpdatePet(PetId, form);
        await OnSave.InvokeAsync(PetId);

        loading = false;
        OnlyRead = true;
        await LoadDataAsync();

        StateHasChanged();
    }

    async void Cancel()
    {
        bool res = await CustomDialogService.OpenConfirmAsync(
            "Se perderán los cambios realizados, ¿Continuar?", 
            "Cancelar", 
            "Continuar", 
            "Cancelar", 
            new DialogOptions { CloseDialogOnEsc = false, ShowClose = false, CloseDialogOnOverlayClick = false }
        );

        if(res == true)
        {
            OnlyRead = true;
        }

        StateHasChanged();
    }

    void Edit()
    {
        OnlyRead = false;
        StateHasChanged();
    }

    void Close()
    {
        DialogService.Close();
    }
}