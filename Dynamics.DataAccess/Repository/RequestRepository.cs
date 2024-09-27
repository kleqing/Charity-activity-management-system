using System.Linq.Expressions;
using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.DataAccess.Repository;

public class RequestRepository : IRequestRepository
{
    private readonly ApplicationDbContext _context;

    public RequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Request>> GetAllAsync()
    {
        return await _context.Requests.ToListAsync();
    }

    public async Task<List<Request>> GetAllRequestWithUsersAsync()
    {
        return await _context.Requests.Include(u => u.User).ToListAsync();
    }

    public Task<Request> GetAsync(Expression<Func<Request, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateAsync(Request project)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Request project)
    {
        throw new NotImplementedException();
    }

    public Task<Request> DeleteAsync(Expression<Func<Request, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}