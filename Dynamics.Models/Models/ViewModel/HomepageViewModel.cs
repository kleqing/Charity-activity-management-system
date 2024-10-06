using Dynamics.Models.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace Dynamics.Models.Models.ViewModel;

public class HomepageViewModel
{
    public IEnumerable<Request>? Requests { get; set; }
    public IEnumerable<ProjectOverviewDto>? OnGoingProjects { get; set; }
    // public IEnumerable<OrganizationOverviewDto> Organizations { get; set; }
    public IEnumerable<ProjectOverviewDto> SuccessfulProjects { get; set; }
}