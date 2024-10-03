using System.Linq.Expressions;
using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.DataAccess.Repository;

public class UserToProjectTransactionHistoryRepository : IUserToProjectTransactionHistoryRepository
{
    private readonly ApplicationDbContext _context;

    public UserToProjectTransactionHistoryRepository(ApplicationDbContext context)
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
        return _context.UserToProjectTransactionHistories.Where(filter).FirstOrDefaultAsync();
    }

    public Task<bool> Add(UserToProjectTransactionHistory entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(UserToProjectTransactionHistory entity)
    {
        throw new NotImplementedException();
    }

    public async Task<UserToProjectTransactionHistory> DeleteTransactionByIdAsync(Guid id)
    {
        var entity = await GetAsync(tr => tr.TransactionID.Equals(id));
        if (entity == null) return null;
        var final = _context.UserToProjectTransactionHistories.Remove(entity);
        await _context.SaveChangesAsync();
        return final.Entity;
    }
}