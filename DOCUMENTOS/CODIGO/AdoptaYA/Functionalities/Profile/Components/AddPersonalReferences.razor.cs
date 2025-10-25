using Microsoft.AspNetCore.Components;

namespace AdoptaYA.Functionalities.Profile.Components;
public partial class AddPersonalReferences
{
    [Parameter] public EventCallback OnReloadData { get; set; }
}