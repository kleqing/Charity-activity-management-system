using Charity_Management_System;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task AddUser(User user)
        {
            await UserDAO.Instance.AddUser(user);
        }

        public async Task DeleteUser(int id)
        {
            await UserDAO.Instance.DeleteUser(id);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await UserDAO.Instance.GetAllUsers();
        }

        public async Task<User> GetUserById(int id)
        {
            return await UserDAO.Instance.GetUserById(id);
        }

        public async Task UpdateUser(User user)
        {
            await UserDAO.Instance.UpdateUser(user);
        }
    }
}
