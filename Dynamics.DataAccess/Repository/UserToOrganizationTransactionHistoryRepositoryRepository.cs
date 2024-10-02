using System.Linq.Expressions;
using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.DataAccess.Repository;

public class UserToOrganizationTransactionHistoryRepositoryRepository : IUserToOrganizationTransactionHistoryRepository
{
    private readonly ApplicationDbContext _context;

    public UserToOrganizationTransactionHistoryRepositoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserToOrganizationTransactionHistory>> GetAllAsync()
    {
        return await _context.UserToOrganizationTransactionHistories.ToListAsync();
    }

    public async Task<List<UserToOrganizationTransactionHistory>> GetAllAsyncWithExpression(
        Expression<Func<UserToOrganizationTransactionHistory, bool>> filter)
    {
        return await _context.UserToOrganizationTransactionHistories.Where(filter)
            .Include(ut => ut.User)
            .Include(ut => ut.OrganizationResource)
            .ThenInclude(ut => ut.Organization)
            .ToListAsync();
    }

    public Task<UserToOrganizationTransactionHistory?> GetAsync(
        Expression<Func<UserToOrganizationTransactionHistory, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Add(UserToOrganizationTransactionHistory entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(UserToOrganizationTransactionHistory entity)
    {
        throw new NotImplementedException();
    }

    public Task<UserToOrganizationTransactionHistory> DeleteById(Guid id)
    {
        throw new NotImplementedException();
    }
}