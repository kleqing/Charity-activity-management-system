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

    /**
     * Included ProjectMember, User, ProjectResource
     */
    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects.Include(p => p.ProjectMember).ThenInclude(u => u.User)
            .Include(p => p.ProjectResource).ToListAsync();
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
}