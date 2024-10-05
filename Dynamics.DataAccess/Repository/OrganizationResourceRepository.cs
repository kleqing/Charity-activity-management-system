using System.Linq.Expressions;
using Dynamics.Models.Models;

namespace Dynamics.DataAccess.Repository;

public class OrganizationResourceRepository : IOrganizationResourceRepository
{
    public Task<List<OrganizationResource>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<OrganizationResource> GetAsync(Expression<Func<OrganizationResource, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateAsync(OrganizationResource project)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(OrganizationResource project)
    {
        throw new NotImplementedException();
    }

    public Task<OrganizationResource> DeleteAsync(Expression<Func<OrganizationResource, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}