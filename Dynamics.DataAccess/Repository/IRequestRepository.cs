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
		Task<List<Request>> GetRequestsAsync();
		Task<List<Request>> GetAllPaginatedAsync(int pageNumber, int pageSize);
		Task<List<Request>> GetAllByIdPaginatedAsync(string role, Guid id, int pageNumber, int pageSize);
		Task<Request> GetAsync(Expression<Func<Request, bool>> filter);
		Task<Request> GetByIdAsync(Expression<Func<Request, bool>> filter, string role, Guid id);
		Task AddAsync(Request entity);
		Task UpdateAsync(Request entity);
		Task DeleteAsync(Request entity);
		// Choose one
		Task<List<Request>> SearchIdFilterAsync(string searchQuery, Guid userId);
		Task<List<Request>> SearchIndexFilterAsync(string searchQuery);
		IQueryable<Request> GetAll(); //get IQueryable for filtering
	}
}
