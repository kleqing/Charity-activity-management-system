using Dynamics.Models.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Dynamics.Models.Models.ViewModel;

public class HomepageViewModel
{
    public IEnumerable<Request>? Requests { get; set; }
    public IEnumerable<ProjectOverviewDto>? Projects { get; set; }
    // public IEnumerable<OrganizationOverviewDto> Organizations { get; set; }

    [Required]
    [DataType(DataType.EmailAddress, ErrorMessage = "Not an email.")]
    public string Test { get; set; }
}