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
		Task<IQueryable<Request>> GetAllAsync();
		Task<IQueryable<Request>> GetAllByIdAsync(string role, Guid id);
		Task<Request> GetAsync(Expression<Func<Request, bool>> filter);
		Task<Request> GetByIdAsync(Expression<Func<Request, bool>> filter, string role, Guid id);
		Task AddAsync(Request entity);
		Task UpdateAsync(Request entity);
		Task DeleteAsync(Request entity);
		Task<IQueryable<Request>> SearchIdFilterAsync(string searchQuery,string filterQuery, Guid userId);
		Task<IQueryable<Request>> SearchIndexFilterAsync(string searchQuery, string filterQuery);
		Task<List<Request>> PaginateAsync(IQueryable<Request> requestQuery, int pageNumber, int pageSize);
		Task<Request?> GetRequestAsync(Expression<Func<Request, bool>> filter, string? includeObjects = null);
	}
}
