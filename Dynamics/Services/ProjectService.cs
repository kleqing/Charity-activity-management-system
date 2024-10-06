using AutoMapper;
using Dynamics.DataAccess;
using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models.Dto;
using Dynamics.Models.Models.DTO;
using Microsoft.Build.Evaluation;
using Microsoft.IdentityModel.Tokens;
using Project = Dynamics.Models.Models.Project;

namespace Dynamics.Services;

public class ProjectService : IProjectService
{
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepo;
    private readonly IProjectResourceRepository _projectResourceRepo;
    private readonly IProjectMemberRepository _projectMemberRepo;
    private readonly ApplicationDbContext _context;

    public ProjectService(IMapper mapper, IProjectRepository projectRepo, IProjectResourceRepository projectResourceRepo,
        IProjectMemberRepository projectMemberRepo, ApplicationDbContext context)
    {
        _mapper = mapper;
        _projectRepo = projectRepo;
        _projectResourceRepo = projectResourceRepo;
        _projectMemberRepo = projectMemberRepo;
        _context = context;
    }
    
    
    public ProjectOverviewDto MapToProjectOverviewDto(Project p)
    {
        if (p.ProjectMember.IsNullOrEmpty()) throw new Exception("WARNING PROJECT MEMBER IS EMPTY");
        var tempProjectOverviewDto = _mapper.Map<ProjectOverviewDto>(p);
        // Get leader project
        var leader = p.ProjectMember.Where(pm => pm.ProjectID == p.ProjectID && pm.Status == 3).FirstOrDefault();
        if (leader == null)
        {
            leader = p.ProjectMember.Where(pm => pm.ProjectID == p.ProjectID && pm.Status == 2).FirstOrDefault();
        }
        if (leader == null) throw new Exception("No leader for project found");
        tempProjectOverviewDto.ProjectLeader = leader.User;
        tempProjectOverviewDto.ProjectMembers = p.ProjectMember.Count(pm => pm.ProjectID == p.ProjectID);
        tempProjectOverviewDto.ProjectProgress = GetProjectProgress(p);
        var moneyRaised = p.ProjectResource.FirstOrDefault(pr => pr.ResourceName.Equals("Money", StringComparison.CurrentCultureIgnoreCase) && pr.ProjectID == p.ProjectID);
        if (moneyRaised != null)
        {
            tempProjectOverviewDto.ProjectRaisedMoney = moneyRaised.Quantity ?? 0;
        }
        tempProjectOverviewDto.Organization = p.Organization;   
        return tempProjectOverviewDto;
    }

    /**
     * Note the param projects has to include the project resource for calculation
     * (Decrypted)
     */
    // public List<Project> GetCompletedProjectsFromProjects(List<Project> projects)
    // {
    //     var result = new List<Project>();
    //     foreach (var project in projects)
    //     {
    //         // Make sure that the money resource is not null
    //         var moneyResource = project.ProjectResource.Where(pr => pr.ResourceName.Equals("Money", StringComparison.CurrentCultureIgnoreCase));
    //         if (!moneyResource.Any()) continue;
    //         var isFull = moneyResource.FirstOrDefault().ExpectedQuantity >= moneyResource.FirstOrDefault().Quantity;
    //         result.Add(project);
    //     }
    //     return result;
    // }
    
    // Use for display purpose (Multiple database trips)
    public int? GetProjectProgressId(Guid projectId)
    {
        var resourceNumbers = _context.ProjectResources
            .Where(p => p.ProjectID == projectId && p.ResourceName.ToLower().Equals("money"))
            .Select(resource => new
            {
                quantity = resource.Quantity,
                expectedQuantity = resource.ExpectedQuantity
            }).FirstOrDefault();
        if (resourceNumbers == null) return -1;
        if (resourceNumbers.expectedQuantity == 0) return 0;
        return resourceNumbers.quantity * 100 / resourceNumbers.expectedQuantity;
    }
    
    /**
     * Make sure the param p already included the project resource
     */
    public int? GetProjectProgress(Project p)
    {
        var resourceNumbers = p.ProjectResource.Where(pr => pr.ResourceName.ToLower().Equals("money"))
            .Select(resource => new
            {
                quantity = resource.Quantity,
                expectedQuantity = resource.ExpectedQuantity
            }).FirstOrDefault();
        if (resourceNumbers == null) return -1;
        if (resourceNumbers.expectedQuantity == 0) return 0;
        return resourceNumbers.quantity * 100 / resourceNumbers.expectedQuantity;
    }
}