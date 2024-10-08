using System.Linq.Expressions;
using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.DataAccess.Repository;

public class ProjectResourceRepository : IProjectResourceRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectResourceRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public Task<IEnumerable<ProjectResource?>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ProjectResource?> GetAsync(Expression<Func<ProjectResource, bool>> predicate)
    {
        return await _context.ProjectResources.Where(predicate).FirstOrDefaultAsync();
    }

    public Task<bool> CreateAsync(ProjectResource project)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(ProjectResource project)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectResource> DeleteAsync(Expression<Func<ProjectResource, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}