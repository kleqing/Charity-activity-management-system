using Dynamics.Models.Models;
using System.Linq.Expressions;

namespace Dynamics.DataAccess.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetAsync(Expression<Func<User, bool>> filter);
        Task<bool> AddAsync(User entity);
        Task<bool> UpdateAsync(User entity);
        Task<User> DeleteById(Guid id);

        /**
         * Check if a user is in what role
         */
        Task<bool> IsUserInRole(Guid id, string roleName);
        
        /**
         * Get role from a user in auth db
         */
        Task<string> GetRoleFromUserAsync(Guid userId);
        /**
         * Add a role to a user in BOTH Auth database and Main database
         */
        Task AddToRoleAsync(Guid userId, string roleName);

        /**
         * Delete a role to a user in BOTH Auth database and Main database
         */
        Task DeleteRoleFromUserAsync(Guid userId, string roleName);
    }
}