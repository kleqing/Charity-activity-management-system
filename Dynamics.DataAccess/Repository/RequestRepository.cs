using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.DataAccess.Repository
{
    public class RequestRepository: IRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public RequestRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IQueryable<Request> GetAll()
        {
            return _db.Requests;
        }

        public async Task<List<Request>> GetAllRequestsAsync(string? includeObjects = null)
        {
            IQueryable<Request> requests = _db.Requests;
            if (!string.IsNullOrEmpty(includeObjects))
            {
                foreach (var includeObj in includeObjects.Split(
                    new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    requests = requests.Include(includeObj);
                }
            }
            return await requests.ToListAsync();
        }

        public  Task<Request?> GetRequestAsync(Expression<Func<Request,bool>> filter, string? includeObjects = null)
        {
            var request = _db.Requests.Where(filter);
            if (!string.IsNullOrEmpty(includeObjects))
            {
                foreach (var includeObj in includeObjects.Split(
                    new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    request = request.Include(includeObj);
                }
            }
            return request.FirstOrDefaultAsync();
        }
    }
}
