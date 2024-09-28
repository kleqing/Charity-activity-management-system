using Dynamics.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dynamics.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> userManager;

        public UserRepository(ApplicationDbContext db, AuthDbContext authDbContext, UserManager<IdentityUser> userManager)
        {
            _db = db;
            this.userManager = userManager;
        }

        // TODO: Decide whether we use one database or 2 database for managing the user
        public async Task<bool> Add(User entity)
        {
            try
            {
                _db.Users.Add(entity);
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
            var user = _db.Users.Find(id);
            if (user != null)
            {
                // TODO NO NO DON'T Delete, BAN HIM INSTEAD
                // _db.Users.Remove(user);
                throw new Exception("TODO: BAN THIS USER INSTEAD");
                await _db.SaveChangesAsync();
            }
            return user;
        }

        public async Task<User?> GetAsync(Expression<Func<User, bool>> filter)
        {
            var user = await _db.Users.Where(filter).FirstOrDefaultAsync();
            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _db.Users.ToListAsync();
            return users;
        }
        //
        public async Task<bool> Update(User user)
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
