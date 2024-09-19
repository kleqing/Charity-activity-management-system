using Dynamics.Models.Models;
using System.Linq.Expressions;

namespace Dynamics.DataAccess.Repository
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User Get(Expression<Func<User, bool>> filter);
        bool Add(User entity);
        bool Update(User entity);
        User DeleteById(Guid id);
    }
}
