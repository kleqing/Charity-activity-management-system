using Dynamics.Models.Models;
using Dynamics.Models.Models.Dto;

namespace Dynamics.Services;

public interface IProjectService
{
    /**
     * Map to dto for display purposes (Card)
     */
    public ProjectOverviewDto MapToProjectOverviewDto(Project p);
}