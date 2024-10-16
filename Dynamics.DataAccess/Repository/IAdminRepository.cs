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
        // 2024-9-27
        // Request
        Task<List<Request>> ViewRequest();
        Task<Request?> GetRequest(Expression<Func<Request, bool>> filter);
        Task<int> ChangeRequestStatus(Guid id);
        public Task<Request> DeleteRequest(Guid id);

        // User   
        Task<List<User>> ViewUser();
        Task<bool> BanUserById(Guid id);
        Task<List<User>> GetTop5User();
        Task<User?> GetUser(Expression<Func<User, bool>> filter);
        public Task ChangeUserRole(Guid id);
        public Task<string> GetUserRole(Guid id);


        // Organization
        Task<List<Organization>> ViewOrganization();
        Task<int> ChnageOrganizationStatus(Guid id);
        Task<List<Organization>> GetTop5Organization();
        Task<Organization?> GetOrganization(Expression<Func<Organization, bool>> filter);

        // 2024-10-16
        Task<Organization?> GetOrganizationInfomation(Expression<Func<Organization, bool>> filter);
        public Task<int> MemberJoinedOrganization(Guid id);


        // 2024-9-30
        // Recent item
        Task<List<Request>> ViewRecentItem();

        // 2024-10-1
        // Count (For Dashboard)
        Task<int> CountUser();
        Task<int> CountOrganization();
        Task<int> CountRequest();
        Task<int> CountProject();

        // 2024-10-2
        // Project
        public Task<List<Project>> ViewProjects();
        //public Task<List<Project>> ViewProjectsDetail(Guid id);
        public Task<Project?> GetProjects(Expression<Func<Project, bool>> filter);
        public Task<bool> BanProject(Guid id);

        // 2024-10-4
        // Report
        Task<List<Report>> ViewReport();
    }
}
