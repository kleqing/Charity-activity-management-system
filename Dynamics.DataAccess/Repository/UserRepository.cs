using Dynamics.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dynamics.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly AuthDbContext _authDbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(ApplicationDbContext db, AuthDbContext authDbContext,
            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _authDbContext = authDbContext;
            this._userManager = userManager;
        }

        // TODO: Decide whether we use one database or 2 database for managing the user
        public async Task<bool> AddAsync(User entity)
        {
            try
            {
                await _db.Users.AddAsync(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<User> DeleteById(Guid id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserID.Equals(id));
            if (user != null)
            {
                // TODO NO NO DON'T Delete, BAN HIM INSTEAD
                // _db.Users.Remove(user);
                throw new Exception("TODO: BAN THIS USER INSTEAD");
                await _db.SaveChangesAsync();
            }

            return user;
        }

        public async Task<bool> IsUserInRole(Guid id, string roleName)
        {
            var authUser = await _userManager.FindByIdAsync(id.ToString());
            if (authUser == null) throw new Exception("GET ROLE FAILED: USER NOT FOUND");
            var result = await _userManager.IsInRoleAsync(authUser, roleName);
            return result;
        }

        public async Task<List<string>> GetRolesFromUserAsync(Guid userId)
        {
            var authUser = await _userManager.FindByIdAsync(userId.ToString());
            if (authUser == null) throw new Exception("GET ROLE FAILED: USER NOT FOUND");
            return _userManager.GetRolesAsync(authUser).GetAwaiter().GetResult().ToList();
        }

        public async Task AddToRoleAsync(Guid userId, string roleName)
        {
            var authUser = await _userManager.FindByIdAsync(userId.ToString());
            if (authUser == null) throw new Exception("ADD ROLE FAILED: USER NOT FOUND");
            await _userManager.AddToRoleAsync(authUser, roleName);
        }

        public async Task DeleteRoleFromUserAsync(Guid userId, string roleName)
        {
            var authUser = await _userManager.FindByIdAsync(userId.ToString());
            if (authUser == null) throw new Exception("DELETE ROLE FAILED: USER NOT FOUND");
            await _userManager.RemoveFromRoleAsync(authUser, roleName);
        }

        public async Task<User?> GetAsync(Expression<Func<User, bool>> filter)
        {
            var user = await _db.Users.Where(filter).FirstOrDefaultAsync();
            return user;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _db.Users.ToListAsync();
            return users;
        }

        //
        public async Task<bool> UpdateAsync(User user)
        {
            var existingItem = await GetAsync(u => user.UserID == u.UserID);
            if (existingItem == null)
            {
                return false;
            }

            // Only update the property that has the same name between 2 models
            _db.Entry(existingItem).CurrentValues.SetValues(user);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}