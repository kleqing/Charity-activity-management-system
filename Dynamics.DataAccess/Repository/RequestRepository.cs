using Azure.Core;
using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Request = Dynamics.Models.Models.Request;

namespace Dynamics.DataAccess.Repository
{
	public class RequestRepository : IRequestRepository
	{
		private readonly ApplicationDbContext _db;
		public RequestRepository(ApplicationDbContext db)
        {
			_db = db;
		}
        public async Task AddAsync(Request entity)
		{
			_db.Add(entity);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteAsync(Request entity)
		{
			var request = await _db.Requests.FirstOrDefaultAsync(r => r.RequestID == entity.RequestID);
			if (request != null)
			{
				_db.Remove(request);
				await _db.SaveChangesAsync();
			}
		}
		
		public Task<IQueryable<Request>> SearchIdFilterAsync(string searchQuery, string filterQuery, Guid userId)
		{
			var requests = _db.Requests.Where(r => r.RequestID == userId);
			switch (filterQuery)
			{
				case "All":
					requests = _db.Requests
						.Where(r => r.RequestTitle.Contains(searchQuery) || r.Content.Contains(searchQuery) || r.Location.Contains(searchQuery))
						.OrderBy(r => r.CreationDate);
					break;
				case "Title":
					requests = _db.Requests
						.Where(r => r.RequestTitle.Contains(searchQuery))
						.OrderBy(r => r.CreationDate);
					break;
				case "Location":
					requests = _db.Requests
						.Where(r => r.Location.Contains(searchQuery))
						.OrderBy(r => r.CreationDate);
					break;
				case "Content":
					requests = _db.Requests
						.Where(r => r.Content.Contains(searchQuery))
						.OrderBy(r => r.CreationDate);
					break;
			}
			return Task.FromResult(requests);
		}

		public Task<IQueryable<Request>> SearchIndexFilterAsync(string searchQuery, string filterQuery)
		{
			var requests = _db.Requests.AsQueryable();
			switch (filterQuery)
			{
				case "All":
					requests = _db.Requests
						.Where(r => r.RequestTitle.Contains(searchQuery) || r.Content.Contains(searchQuery) || r.Location.Contains(searchQuery))
						.OrderBy(r => r.CreationDate);
					break;
				case "Title":
					requests = _db.Requests
						.Where(r => r.RequestTitle.Contains(searchQuery))
						.OrderBy(r => r.CreationDate);
					break;
				case "Location":
					requests = _db.Requests
						.Where(r => r.Location.Contains(searchQuery))
						.OrderBy(r => r.CreationDate);
					break;
				case "Content":
					requests = _db.Requests
						.Where(r => r.Content.Contains(searchQuery))
						.OrderBy(r => r.CreationDate);
					break;
			}
			return Task.FromResult(requests);
		}

		public async Task<Request> GetAsync(Expression<Func<Request, bool>> filter)
		{
			var query = await _db.Requests.Include(r => r.User).Where(filter).FirstOrDefaultAsync();
			return query;
		}

		public async Task<List<Request>> GetRequestsAsync()
		{
			// var test = _db.Requests.ToList();
			return await _db.Requests.Include(r => r.User).ToListAsync(); 
		}

		public Task<IQueryable<Request>> GetAllAsync()
		{
			// var req = _db.Requests.ToList();
			var requests = _db.Requests
													.Include(r => r.User)
													.AsQueryable();
			return Task.FromResult(requests);
		}

		public async Task UpdateAsync(Request entity)
		{
			var request = await GetAsync(r => entity.RequestID == r.RequestID);
			if (request != null)
			{
				_db.Requests.Update(request);
				await _db.SaveChangesAsync();
			}
		}

		public Task<IQueryable<Request>> GetAllByIdAsync(string role, Guid id)
		{
			var query = _db.Requests.AsQueryable();
			if (role == "User")
			{ 
				 query = query.Where(r => r.UserID == id);
			}
			return Task.FromResult(query);
		}

		public async Task<List<Request>> PaginateAsync(IQueryable<Request> requestQuery, int pageNumber, int pageSize)
		{
			return await requestQuery
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<Request> GetByIdAsync(Expression<Func<Request, bool>> filter, string role, Guid id)
		{
			var query = _db.Requests.Include(r => r.User).Where(filter).AsQueryable();
			var request = await query.Where(filter).FirstOrDefaultAsync();
			if (role == "User" && request.UserID == id)
			{
				return request;
			}
			else if (role == "Admin")
			{
				return request;
			}
			return null;
		}
		
        public Task<Request?> GetRequestAsync(Expression<Func<Request,bool>> filter, string? includeObjects = null)
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

        public Task<IQueryable<Request>> GetRequestDateFilterAsync(IQueryable<Request> requests, DateOnly dateFrom, DateOnly dateTo)
        {
	        return Task.FromResult(requests.Where(r => r.CreationDate >= dateFrom && r.CreationDate <= dateTo));
        }
	}
}
