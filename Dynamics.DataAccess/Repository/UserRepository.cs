using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dynamics.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
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
                _db.Users.Remove(user);
            }
            return user;
        }

        public async Task<User> Get(Expression<Func<User, bool>> filter)
        {
            var user = _db.Users.Where(filter).FirstOrDefault();
            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _db.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.UserEmail == email);
        }

        public async Task<bool> Update(User user)
        {
            var existingItem = await Get(u => user.UserID == u.UserID);
            if (existingItem == null)
            {
                return false;
            }
            existingItem.UserFullName = user.UserFullName;
            existingItem.UserDOB = user.UserDOB;
            existingItem.UserPhoneNumber = user.UserPhoneNumber;
            existingItem.UserAddress = user.UserAddress;
            existingItem.UserAvatar = user.UserAvatar;
            existingItem.UserDescription = user.UserDescription;
            _db.Users.Update(existingItem);
            await _db.SaveChangesAsync();
            return true;
        }

        
        
    }
}
