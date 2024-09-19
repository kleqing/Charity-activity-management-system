using BussinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserDAO : SingleOnBase<UserDAO>
    {
        // Get all users
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }
        // Find user by id
        public async Task<User> GetUserById(int id)
        {
            _context = new DBContext();
            var userID = await _context.Users.FirstOrDefaultAsync(u => u.userID == id);
            if (userID == null)
            {
                return null;
            }
            return userID;
        }
        // Add user
        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        // Update user
        public async Task UpdateUser(User user)
        {
            var existingItem = await GetUserById(user.userID);
            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(user); 
            }
            else
            {
                _context.Users.Add(user);
            }
            await _context.SaveChangesAsync();
        }
        // Delete user
        public async Task DeleteUser(int id)
        {
            var user = await GetUserById(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
