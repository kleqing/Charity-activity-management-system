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
		Task<List<Request>> GetAllAsync();
		Task<List<Request>> GetAllByRoleAsync(string role, string id);
		Task<Request> GetAsync(Expression<Func<Request, bool>> filter);
		Task<Request> GetByRoleAsync(Expression<Func<Request, bool>> filter, string role, string id);
		Task AddAsync(Request entity);
		Task UpdateAsync(Request entity);
		Task DeleteAsync(Request entity);
	}
}
