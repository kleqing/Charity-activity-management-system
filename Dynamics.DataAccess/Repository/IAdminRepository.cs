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
        Task<IEnumerable<Request>> ViewRequest();
        Task UpdateRequest(Request request);
        Task<Request> GetRequestByID(Guid id);

        // User   
        Task<IEnumerable<User>> ViewUser();
        Task<bool> BanUserById(Guid id);
        Task<IEnumerable<User>> GetTop5User();

        // Organization
        Task<IEnumerable<Organization>> ViewOrganization();
        Task<bool> BanOrganizationById(Guid id);
        Task<IEnumerable<Organization>> GetTop5Organization();


    }
}
