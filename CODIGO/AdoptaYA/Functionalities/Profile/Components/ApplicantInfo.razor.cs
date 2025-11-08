using AdoptaYA.Functionalities.Profile.Model;
using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace AdoptaYA.Functionalities.Profile.Components;
public partial class ApplicantInfo
{
    [Parameter] public ApplicantResponseDTO applicant { get; set; }
}