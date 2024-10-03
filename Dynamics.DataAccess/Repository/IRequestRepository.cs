using Dynamics.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.DataAccess.Repository
{
    public interface IRequestRepository
    {
        Task<List<Request>> GetAllRequestsAsync(string? includeObjects = null);
        IQueryable<Request> GetAll();//get IQueryable for filtering
        Task<Request> GetRequestAsync(Expression<Func<Request,bool>> filter, string? includeObjects = null);

    }
}
