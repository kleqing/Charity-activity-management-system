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
        return await _context.ProjectMembers.Where(predicate).Include(p => p.Project).ToListAsync();
    }

    public async Task<ProjectMember?> GetAsync(Expression<Func<ProjectMember, bool>>? predicate)
    {
        if (predicate is null)
        {
            return await _context.ProjectMembers.Include(pm => pm.User).FirstOrDefaultAsync();
        }
        return await _context.ProjectMembers.Where(predicate).Include(pm => pm.User).FirstOrDefaultAsync();
    }

    public Task<bool> CreateAsync(ProjectMember project)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(ProjectMember project)
    {
        throw new NotImplementedException();
    }

    public async Task<ProjectMember> DeleteAsync(Expression<Func<ProjectMember, bool>> predicate)
    {
        var target = await GetAsync(predicate);
        if (target is null) return null;
        var final = _context.ProjectMembers.Remove(target);
        await _context.SaveChangesAsync();
        return final.Entity;
    }
}