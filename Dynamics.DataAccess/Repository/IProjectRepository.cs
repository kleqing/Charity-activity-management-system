using System.Linq.Expressions;
using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.DataAccess.Repository
{
    public interface IProjectRepository
    {
        // Use for display purposes - Kiet
        Task<List<Project>> GetAllAsync();
        Task<List<Project>> GetAllProjectsAsync(string? includeObjects = null);

        //using this to get project member of leader
        Task<User> GetProjectLeaderAsync(Guid projectID, string? includeObjects = null);

        //using this to view detail project
        Task<List<string>> GetStatisticOfProject(Guid projectID);
        Task<Project?> GetProjectAsync(Expression<Func<Project, bool>> filter, string? includeObjects = null);
        Task<bool> UpdateAsync(Project entity, Guid newProjectLeaderID);
        Task<bool> ShutdownProjectAsync(ShutdownProjectVM entity);
        Task<bool> FinishProjectAsync(FinishProjectVM entity);

        Task<bool> SendReportProjectRequestAsync(Report entity);

        //manage member of project
        List<ProjectMember> FilterProjectMember(Expression<Func<ProjectMember, bool>> filter,
            string? includeObjects = null);

        List<User> FilterMemberOfProject(Expression<Func<ProjectMember, bool>> filter);

        Task<bool> DeleteMemberProjectByIdAsync(Guid memberID, Guid projectID);

        //manage transaction history of project
        Task<List<UserToProjectTransactionHistory>> GetRandom5Donors(Guid projectID);

        Task<List<UserToProjectTransactionHistory>> GetAllUserDonateAsync(
            Expression<Func<UserToProjectTransactionHistory, bool>> filter);

        Task<List<OrganizationToProjectHistory>> GetAllOrganizationDonateAsync(
            Expression<Func<OrganizationToProjectHistory, bool>> filter);

        Task<bool> AcceptedUserDonateRequestAsync(Guid transactionID);
        Task<bool> DenyUserDonateRequestAsync(Guid transactionID);
        Task<bool> AcceptedOrgDonateRequestAsync(Guid transactionID);
        Task<bool> DenyOrgDonateRequestAsync(Guid transactionID);

        Task<string> GetAllImagesAsync(Guid id, string owner);

        //manage member request
        Task<bool> AddJoinRequest(Guid memberID, Guid projectID);
        Task<bool> AcceptedJoinRequestAsync(Guid memberID, Guid projectID);

        Task<bool> DenyJoinRequestAsync(Guid memberID, Guid projectID);

        //handle resource statistic
        Task<bool> HandleResourceAutomatic(Guid transactionID, string donor);
        Task<IEnumerable<ProjectResource?>> FilterProjectResourceAsync(Expression<Func<ProjectResource, bool>> filter);
        IQueryable<ProjectResource> GetAllProjectResourceQuery();
        Task<bool> AddResourceTypeAsync(ProjectResource entity);
        Task<bool> UpdateResourceTypeAsync(ProjectResource entity);

        Task<bool> DeleteResourceTypeAsync(Guid resourceID);

        //manage project update
        Task<List<History>?> GetAllPhaseReportsAsync(Expression<Func<History, bool>> filter,
            string? includeObjects = null);

        Task<bool> AddPhaseReportAsync(History entity);

        Task<bool> EditPhaseReportAsync(History entity);

        Task<bool> DeletePhaseReportAsync(Guid id);

        //constraint project update
        Task<List<DateTime>> GetExistingReportDatesAsync(Guid projectID);
        Task<DateTime> GetReportDateAsync(Guid projectID, Guid historyID);
        Task<bool> AddUserDonateRequestAsync(UserToProjectTransactionHistory? userDonate);
        Task<bool> AddOrgDonateRequestAsync(OrganizationToProjectHistory? orgDonate);
        Task<int> CountMembersOfProjectByIdAsync(Guid projectId);

        //Repose ò tuan
        Task<List<Project>> GetAllProjectsByOrganizationIDAsync(Expression<Func<Project, bool>> filter);

        Task<bool> AddProjectAsync(Project entity);

        Task<bool> AddProjectMemberAsync(ProjectMember entity);

        Task<bool> AddProjectResourceAsync(ProjectResource entity);
        Task<Project> GetProjectByProjectIDAsync(Expression<Func<Project, bool>> filter);

        Task<List<ProjectResource>> GetAllResourceByProjectIDAsync(Expression<Func<ProjectResource, bool>> filter);

        Task<bool> UpdateProjectResource(ProjectResource entity);
        
        //Pagination
        Task<List<Project>> PaginatedAsync (IQueryable<Project> projectQuery, int pageNumber, int pageSize);
    }
}
