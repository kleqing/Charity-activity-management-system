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
    public class AdminRepository : IAdminRepository
    {
        public readonly ApplicationDbContext _db;
        public AdminRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // Organization
        public async Task<bool> BanOrganizationById(Guid id)
        {
            var user = await _db.Organizations.FindAsync(id);
            if (user != null)
            {
                user.isBanned = true;  // Đặt trạng thái bị ban thành true
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
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
                .OrderByDescending(x => x.ProjectCount)
                .Take(5)
                .ToListAsync();

            // Get the detailed information of the top 5 users
            var orgID = TopOrganizations.Select(x => x.OrganizationID).ToList();
            var organization = await _db.Organizations
                .Where(u => orgID.Contains(u.OrganizationID))
                .ToListAsync();

            return organization;
        }

        public async Task<List<Organization>> ViewOrganization()
        {
            var organizations = await _db.Organizations.ToListAsync();
            return organizations;
        }

        // Request

        public async Task<List<Request>> ViewRequest()
        {
            var request = await _db.Requests.ToListAsync();
            return request;
        }

        public async Task<Request?> Get(Expression<Func<Request, bool>> filter)
        {
            var request = await _db.Requests.Where(filter).FirstOrDefaultAsync();
            return request;
        }

        public async Task<bool> UpdateRequest(Request request)
        {
            var existingItem = await Get(r => r.RequestID == request.RequestID);
            if (existingItem == null)
            {
                return false;
            }
            // Only update the property that has the same name between 2 models
            _db.Entry(existingItem).CurrentValues.SetValues(request);
            await _db.SaveChangesAsync();
            return true;
        }

        // User

        public async Task<List<User>> ViewUser()
        {
            var users = await _db.Users.ToListAsync();
            return users;
        }

        public async Task<List<User>> GetTop5User()
        {
            // Group by UserID in ProjectMember to count the number of projects each user participates in
            var topUsers = await _db.ProjectMembers
                .GroupBy(pm => pm.UserID)                      // Group userID
                .Select(g => new
                {
                    UserID = g.Key,
                    ProjectCount = g.Count()                   // Count if user is in project or not
                })
                .OrderByDescending(x => x.ProjectCount)
                .Take(5)
                .ToListAsync();

            // Get the detailed information of the top 5 users
            var userIds = topUsers.Select(x => x.UserID).ToList();
            var users = await _db.Users
                .Where(u => userIds.Contains(u.UserID))
                .ToListAsync();

            return users;
        }
        public async Task<bool> BanUserById(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                user.isBanned = true;  // Đặt trạng thái bị ban thành true
                await _db.SaveChangesAsync();
                return true;
            }
            return false;  // Trả về false nếu không tìm thấy user
        }
    }
}
