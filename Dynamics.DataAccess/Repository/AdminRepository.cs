using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.DataAccess.Repository
{
    public class AdminRepository : IAdminRepository
    {
        public readonly ApplicationDbContext _db;
        public readonly IMemoryCache _memoryCache;
        private const string UserCountCacheKey = "PreviousUserCount";
        public AdminRepository(ApplicationDbContext db, IMemoryCache memoryCache)
        {
            _db = db;
            _memoryCache = memoryCache;
        }

        // ---------------------------------------
        // Organization (View, Ban, Top 5)
        public async Task<Organization?> GetOrganization(Expression<Func<Organization, bool>> filter)
        {
            var org = await _db.Organizations.Where(filter).FirstOrDefaultAsync();
            if (org == null)
            {
                return null;
            }
            return org;
        }

        public async Task<bool> BanOrganizationById(Guid id)
        {
            var org = await GetOrganization(c => c.OrganizationID == id);
            if (org != null)
            {
                org.isBanned = !org.isBanned;
                _db.SaveChangesAsync();
                return org.isBanned;
            }
            return org.isBanned;
        }

        public async Task<List<Organization>> GetTop5Organization()
        {
            var TopOrganizations = await _db.Projects
                .GroupBy(pm => pm.OrganizationID)                      // Group OrganizationID
                .Select(g => new
                {
                    OrganizationID = g.Key,
                    ProjectCount = g.Count()                   // Count if user is in project or not
                })
                .OrderBy(x => x.ProjectCount)
                .Take(5)
                .ToListAsync();

            // Get the detailed information of the top 5 users
            var orgID = TopOrganizations.Select(x => x.OrganizationID).ToList();
            var organization = await _db.Organizations
                .Where(u => orgID.Contains(u.OrganizationID))
                .ToListAsync();

            foreach (var org in organization)
            {
                org.ProjectCount = TopOrganizations.FirstOrDefault(x => x.OrganizationID == org.OrganizationID)?.ProjectCount ?? 0;
            }
            return organization;
        }

        public async Task<List<Organization>> ViewOrganization()
        {
            var organizations = await _db.Organizations.ToListAsync();
            return organizations;
        }

        // ---------------------------------------
        // Request (View, Update)

        public async Task<List<Request>> ViewRequest()
        {
            var request = await _db.Requests.ToListAsync();
            return request;
        }

        public async Task<Request> GetRequestByID(Guid id)
        {
            var request = await _db.Requests.FirstOrDefaultAsync(c => c.RequestID == id);
            if (request == null)
            {
                return null;
            }
            return request;
        }

        public async Task UpdateRequest(Request request)
        {
            var existingItem = await GetRequestByID(request.RequestID);
            if (existingItem == null)
            {
                return;
            }
            _db.Requests.Update(request);
            await _db.SaveChangesAsync();
        }

        // ---------------------------------------
        // User (View, Ban, Top 5)

        public async Task<List<User>> ViewUser()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<List<User>> GetTop5User()
        {
            // Group by UserID in ProjectMember to count the number of projects each user participates in
            var topUsers = await _db.ProjectMembers
                .GroupBy(pm => pm.UserID)    // Group by UserID
                .Select(g => new
                {
                    UserID = g.Key,
                    ProjectCount = g.Count()  // Count how many projects each user is in
                })
                .OrderByDescending(x => x.ProjectCount)  // Order by project count (desc)
                .Take(5)    // Take the top 5
                .ToListAsync();

            // Get the detailed information of the top 5 users
            var userIds = topUsers.Select(x => x.UserID).ToList();
            var users = await _db.Users
                .Where(u => userIds.Contains(u.UserID))  // Find users in the top list
                .ToListAsync();

            // Add the project count to each user (manually map the project count)
            foreach (var user in users)
            {
                user.ProjectCount = topUsers.FirstOrDefault(x => x.UserID == user.UserID)?.ProjectCount ?? 0;
            }

            return users;
        }

        public async Task<bool> BanUserById(Guid id)
        {
            var user = await GetUser(u => id == u.UserID);
            if (user != null)
            {
                user.isBanned = !user.isBanned;
                _db.SaveChangesAsync();
                return user.isBanned;  // Return ban value (true/false)
            }
            return user.isBanned;
        }

        public async Task<User?> GetUser(Expression<Func<User, bool>> filter)
        {
            var user = await _db.Users.Where(filter).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            return user;
        }

        // View Recent request (Recent item in dashoard page)
        public async Task<List<Request>> ViewRecentItem()
        {
            return await _db.Requests.Include(r => r.User).OrderByDescending(x => x.CreationDate).Take(7).ToListAsync();
        }

        // Count (count number of user, organization, request, project in database)
        public async Task<int> CountUser()
        {
            return await _db.Users.CountAsync();
        }

        public async Task<int> CountOrganization()
        {
            return await _db.Organizations.CountAsync();
        }

        public async Task<int> CountRequest()
        {
            return await _db.Requests.CountAsync();
        }

        public async Task<int> CountProject()
        {
            return await _db.Projects.CountAsync();
        }
    }
}
