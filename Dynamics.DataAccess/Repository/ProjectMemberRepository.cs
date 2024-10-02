using System.Linq.Expressions;
using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.DataAccess.Repository;

public class ProjectMemberRepository : IProjectMemberRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectMemberRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<ProjectMember>> GetAllAsync(Expression<Func<ProjectMember, bool>>? predicate = null)
    {
        if (predicate is null)
        {
            return await _context.ProjectMembers.Include(p => p.Project).ToListAsync();
        }
        else
        {
            return await _context.ProjectMembers.Where(predicate).Include(p => p.Project).ToListAsync();
        }
    }

    public Task<ProjectMember?> GetAsync(Expression<Func<ProjectMember, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateAsync(ProjectMember project)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(ProjectMember project)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectMember> DeleteAsync(Expression<Func<ProjectMember, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}