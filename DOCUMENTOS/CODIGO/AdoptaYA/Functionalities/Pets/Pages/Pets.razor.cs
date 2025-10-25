using AdoptaYA.Functionalities.Pets.Components;

namespace AdoptaYA.Functionalities.Pets.Pages;
public partial class Pets
{
    private bool loading = false;
    private int TabsSelectedIndex = 0;

    PetsList petListRef;

    async Task ChangeTab(int index)
    {
        TabsSelectedIndex = index;
        if (index == 0)
        {
            await CallLoadDataPetListAsync();
        }
    }

    private async Task CallLoadDataPetListAsync()
    {
        await petListRef.LoadDataAsync();
    }

}