using System.Linq.Expressions;
using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.DataAccess.Repository;

public interface IUserToProjectTransactionHistoryRepository
{
    Task<List<UserToProjectTransactionHistory>> GetAllAsync();
    Task<List<UserToProjectTransactionHistory>> GetAllAsyncWithExpression(Expression<Func<UserToProjectTransactionHistory, bool>> filter);

    Task<UserToProjectTransactionHistory?> GetAsync(Expression<Func<UserToProjectTransactionHistory, bool>> filter);
    Task<bool> Add(UserToProjectTransactionHistory entity);
    Task<bool> Update(UserToProjectTransactionHistory entity);
    Task<UserToProjectTransactionHistory> DeleteById(Guid id);
}

