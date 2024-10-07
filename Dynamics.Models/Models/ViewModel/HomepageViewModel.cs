using System.Collections;
using Dynamics.Models.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace Dynamics.Models.Models.ViewModel;

public class HomepageViewModel
{
    public IEnumerable<RequestOverviewDto>? Requests { get; set; }
    public IEnumerable<ProjectOverviewDto>? OnGoingProjects { get; set; }
    // public IEnumerable<OrganizationOverviewDto> Organizations { get; set; }
    public IEnumerable<ProjectOverviewDto>? SuccessfulProjects { get; set; }
    public IEnumerable<OrganizationOverviewDto>? Organizations { get; set; }
}