using System.Linq.Expressions;
using Dynamics.Models.Models;

namespace Dynamics.DataAccess.Repository;

public interface IOrganizationRepository
{
    Task<IEnumerable<Organization>> GetAllAsync();
    Task<Organization> GetAsync(Expression<Func<Organization, bool>> predicate);
    Task<bool> CreateAsync(Organization project);
    Task<bool> UpdateAsync(Organization project);
    Task<Organization> DeleteAsync(Expression<Func<Organization, bool>> predicate);
}