using System.Linq.Expressions;
using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.DataAccess.Repository;

public class UserToProjectTransactionHistoryRepositoryRepository : IUserToProjectTransactionHistoryRepository
{
    private readonly ApplicationDbContext _context;

    public UserToProjectTransactionHistoryRepositoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<UserToProjectTransactionHistory>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<UserToProjectTransactionHistory>> GetAllAsyncWithExpression(
        Expression<Func<UserToProjectTransactionHistory, bool>> filter)
    {
        return await _context.UserToProjectTransactionHistories.Where(filter).Include(ut => ut.User).ToListAsync();
    }

    public Task<UserToProjectTransactionHistory?> GetAsync(
        Expression<Func<UserToProjectTransactionHistory, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Add(UserToProjectTransactionHistory entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(UserToProjectTransactionHistory entity)
    {
        throw new NotImplementedException();
    }

    public Task<UserToProjectTransactionHistory> DeleteById(Guid id)
    {
        throw new NotImplementedException();
    }
}