using Dynamics.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.DataAccess.Repository
{
    public interface IAdminRepository
    {
        // Request
        Task<List<Request>> ViewRequest();
        Task UpdateRequest(Request request);
        Task<Request> GetRequestByID(Guid id);

        // User   
        Task<List<User>> ViewUser();
        Task<bool> BanUserById(Guid id);
        Task<List<User>> GetTop5User();
        public Task<User?> GetUser(Expression<Func<User, bool>> filter);

        // Organization
        Task<List<Organization>> ViewOrganization();
        Task<bool> BanOrganizationById(Guid id);
        Task<List<Organization>> GetTop5Organization();
        public Task<Organization?> GetOrganization(Expression<Func<Organization, bool>> filter);

        // Recent item
        Task<List<Request>> ViewRecentItem();

        // Count (For Dashboard)
        Task<int> CountUser();
        Task<int> CountOrganization();
        Task<int> CountRequest();
        Task<int> CountProject();

    }
}
