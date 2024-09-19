using Dynamics.Models.Models;
using System.Linq.Expressions;

namespace Dynamics.DataAccess.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User> Get(Expression<Func<User, bool>> filter);
        Task<bool> Add(User entity);
        Task<bool> Update(User entity);
        Task<User> DeleteById(Guid id);
    }
}
