using AdoptaYA.Functionalities.Profile.Model;
using Microsoft.AspNetCore.Components;

namespace AdoptaYA.Functionalities.Profile.Components;
public partial class ProfileInfo
{
    [Parameter] public ProfileResponseDTO profile { get; set; }

}