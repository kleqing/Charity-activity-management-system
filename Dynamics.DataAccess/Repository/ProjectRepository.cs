using System.Linq.Expressions;
using Dynamics.Models.Models;
using Dynamics.Models.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.DataAccess.Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects.ToListAsync();
    }

    public Task<Project> GetAsync(Expression<Func<Project, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateAsync(Project project)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Project project)
    {
        throw new NotImplementedException();
    }

    public Task<Project> DeleteAsync(Expression<Func<Project, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CountMembersOfProjectByIdAsync(Guid projectId)
    {
        
        return await _context.ProjectMembers.CountAsync(p => p.ProjectID == projectId);
    }

    public int? GetProjectProgressById(Guid projectId)
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
}