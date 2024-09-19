using Dynamics.Models.Models;
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
        public bool Add(User entity)
        {
            try
            {
                _db.Users.Add(entity);
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public User DeleteById(Guid id)
        {
            var user = _db.Users.Find(id);
            if (user != null)
            {
                _db.Users.Remove(user);
            }
            return user;
        }

        public User Get(Expression<Func<User, bool>> filter)
        {
            var user = _db.Users.Where(filter).FirstOrDefault();
            return user;
        }

        public List<User> GetAll()
        {
            var users = _db.Users.ToList();
            return users;
        }

        public bool Update(User entity)
        {
            var user = _db.Users.Find(entity.Id);
            if (user != null)
            {
                _db.Users.Update(entity);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
