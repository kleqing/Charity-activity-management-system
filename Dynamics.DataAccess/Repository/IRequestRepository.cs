using Dynamics.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dynamics.Utility;

namespace Dynamics.DataAccess.Repository
{
	public interface IRequestRepository
	{
		public Task<List<Request>> GetRequestsAsync();
		/**
		 * Get all but paginated <br/>
		 * Default to page 1, 9 entries <br/>
		 * Return: List of request
		 */
		Task<List<Request>> GetAllAsync(int pageNumber = 1, int pageSize = 9);
		int CountRequests(Expression<Func<Request, bool>>? predicate = null);
		Task<IQueryable<Request>> GetAllByIdAsync(string role, Guid id);
		Task<Request> GetAsync(Expression<Func<Request, bool>> filter);
		Task<Request> GetByIdAsync(Expression<Func<Request, bool>> filter, string role, Guid id);
		Task AddAsync(Request entity);
		Task UpdateAsync(Request entity);
		Task DeleteAsync(Request entity);
		/**
		 * Get all with possible expression
		 */
		public IQueryable<Request> SearchIdFilter(string searchQuery,string filterQuery, Guid userId);
		public IQueryable<Request> SearchIndexFilterAsync(string searchQuery, string filterQuery);
		public Task<List<Request>> PaginateAsync(IQueryable<Request> requestQuery, int pageNumber, int pageSize);
	}
}
