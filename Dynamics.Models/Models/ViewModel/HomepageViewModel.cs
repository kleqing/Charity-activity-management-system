using Dynamics.Models.Models.Dto;

namespace Dynamics.Models.Models.ViewModel;

public class HomepageViewModel
{
    public IEnumerable<Request> Requests { get; set; }
    public IEnumerable<ProjectOverviewDto> Projects { get; set; }
    // public IEnumerable<OrganizationOverviewDto> Organizations { get; set; }
}