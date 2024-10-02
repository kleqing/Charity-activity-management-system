using System.Linq.Expressions;
using Dynamics.Models.Models;

namespace Dynamics.DataAccess.Repository;

public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync();
    Task<Project> GetAsync(Expression<Func<Project, bool>> predicate);
    Task<bool> CreateAsync(Project project);
    Task<bool> UpdateAsync(Project project);
    Task<Project> DeleteAsync(Expression<Func<Project, bool>> predicate);
    Task<int> CountMembersOfProjectByIdAsync(Guid projectId);
    int? GetProjectProgressById(Guid projectId);
}