using System.Text.Json;
using AdoptaYA.Functionalities.Profile.Components;
using AdoptaYA.Functionalities.Profile.Http;
using AdoptaYA.Functionalities.Profile.Model;
using AdoptaYA.Services.Security;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace AdoptaYA.Functionalities.Profile.Pages;
public partial class Profile
{
    [Inject] GetCurrentUser GetCurrentUser { get; set; } = default!;
    [Inject] ProfileHttp ProfileHttp { get; set; } = default!;

    private PersonalReferences PersonalReferencesRef;

    private bool loading = false;

    RadzenTabs tabs;
    int selectedIndex = 0;

    private ProfileResponseDTO profile = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        loading = true;
        var user = await GetCurrentUser.GetUserInfoAsync();
        profile = await ProfileHttp.GetProfileAsync(user.UserId!.Value);
        loading = false;

        StateHasChanged();
    }

    private async Task ReloadDataAsync()
    {
        await LoadDataAsync();
        await PersonalReferencesRef.ReloadDataAsync();

        StateHasChanged();
    }

}