using System.Linq.Expressions;
using Dynamics.Models.Models;

namespace Dynamics.DataAccess.Repository;

public interface IRequestRepository
{
    Task<List<Request>> GetAllAsync();
    Task<Request> GetAsync(Expression<Func<Request, bool>> predicate);
    Task<List<Request>> GetAllRequestWithUsersAsync();
    Task<bool> CreateAsync(Request project);
    Task<bool> UpdateAsync(Request project);
    Task<Request> DeleteAsync(Expression<Func<Request, bool>> predicate);
}