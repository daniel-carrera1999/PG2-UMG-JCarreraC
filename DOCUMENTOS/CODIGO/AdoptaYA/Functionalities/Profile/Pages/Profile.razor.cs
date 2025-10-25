using System.Text.Json;
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

    private bool loading = false;

    RadzenTabs tabs;
    int selectedIndex = 0;

    private ProfileResponseDTO profile = new();

    protected override async Task OnInitializedAsync()
    {
        loading = true;
        await LoadDataAsync();
        loading = false;

        StateHasChanged();
    }

    private async Task LoadDataAsync()
    {
        var user = await GetCurrentUser.GetUserInfoAsync();
        profile = await ProfileHttp.GetProfileAsync(user.UserId!.Value);
    }

}