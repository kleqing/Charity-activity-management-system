using System.Linq.Expressions;
using Dynamics.Models.Models;

namespace Dynamics.DataAccess.Repository;

public interface IProjectResourceRepository
{
    Task<IEnumerable<ProjectResource?>> GetAllAsync();
    Task<ProjectResource?> GetAsync(Expression<Func<ProjectResource, bool>> predicate);
    Task<bool> CreateAsync(ProjectResource project);
    Task<bool> UpdateAsync(ProjectResource project);
    Task<ProjectResource> DeleteAsync(Expression<Func<ProjectResource, bool>> predicate);
}