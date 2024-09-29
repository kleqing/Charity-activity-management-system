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
	//TODO make methods for get user request (all and individual) and update edit and delete
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
		
		public async Task<List<Request>> SearchIdFilterAsync(string searchQuery, Guid userId)
		{
			var requests = await _db.Requests
				.Where(r => r.RequestTitle.Contains(searchQuery) || r.Content.Contains(searchQuery) || r.Location.Contains(searchQuery)
					&& r.UserID == userId).ToListAsync();
			return requests;
		}

		public async Task<Request> GetAsync(Expression<Func<Request, bool>> filter)
		{
			var query = await _db.Requests.Where(filter).FirstOrDefaultAsync();
			return query;
		}

		public async Task<List<Request>> GetAllPaginatedAsync(int pageNumber, int pageSize)
		{
			var requests = await _db.Requests.ToListAsync();
			return requests;
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

		public async Task<List<Request>> GetAllByRolePaginatedAsync(string role, Guid id, int pageNumber, int pageSize)
		{
			var query = _db.Requests.AsQueryable();
			if (role == "User")
			{ 
				 query = query.Where(r => r.UserID == id);
			}
			return await query
				.OrderBy(r => r.CreationDate)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<Request> GetByRoleAsync(Expression<Func<Request, bool>> filter, string role, Guid id)
		{
			var query = await _db.Requests.Where(filter).FirstOrDefaultAsync();
			if (role == "User" && query.UserID == id)
			{
				return query;
			}
			else if (role == "Admin")
			{
				return query;
			}
			return null;
		}
	}
}
