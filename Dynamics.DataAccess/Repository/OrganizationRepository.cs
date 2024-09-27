using System.Linq.Expressions;
using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Dynamics.DataAccess.Repository;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly ApplicationDbContext _context;

    public OrganizationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Organization>> GetAllAsync()
    {
        return await _context.Organizations.ToListAsync();
    }

    public Task<Organization> GetAsync(Expression<Func<Organization, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateAsync(Organization project)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Organization project)
    {
        throw new NotImplementedException();
    }

    public Task<Organization> DeleteAsync(Expression<Func<Organization, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}