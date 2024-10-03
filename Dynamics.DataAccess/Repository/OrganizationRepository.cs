using System.Linq.Expressions;
using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Dynamics.DataAccess.Repository;

public class OrganizationRepository : IOrganizationResourceRepository
{
    private readonly ApplicationDbContext _context;

    public OrganizationRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public Task<List<OrganizationResource>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<OrganizationResource?> GetAsync(Expression<Func<OrganizationResource, bool>> predicate)
    {
        return await _context.OrganizationResources.FirstOrDefaultAsync(predicate);
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