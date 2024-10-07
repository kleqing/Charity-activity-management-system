using Dynamics.Models.Models;
using Dynamics.Models.Models.DTO;

namespace Dynamics.Services;

public interface IProjectService
{
    /**
     * Map to dto for display purposes (Card)
     */
    public ProjectOverviewDto MapToProjectOverviewDto(Project p);
    List<ProjectOverviewDto> MapToListProjectOverviewDto(List<Project> projects);

}