using System.Linq.Expressions;
using Dynamics.Models.Models;

namespace Dynamics.DataAccess.Repository;

public interface IProjectMemberRepository
{
    Task<List<ProjectMember>> GetAllAsync(Expression<Func<ProjectMember, bool>>? predicate = null);
    Task<ProjectMember?> GetAsync(Expression<Func<ProjectMember, bool>> predicate);
    Task<bool> CreateAsync(ProjectMember project);
    Task<bool> UpdateAsync(ProjectMember project);
    Task<ProjectMember> DeleteAsync(Expression<Func<ProjectMember, bool>> predicate);
}