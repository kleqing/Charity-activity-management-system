using Dynamics.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.DataAccess.Repository
{
    public interface IOrganizationRepository
    {
        Task<List<Organization>> GetAllOrganizationsAsync();
        Task<Organization?> GetAsync(Expression<Func<Organization, bool>> filter);
        Task<List<Organization>> GetListOrganizationsByUserIDAsync(Expression<Func<Organization, bool>> filter);
        Task<bool> AddAsync(Organization entity);
        Task<bool> UpdateAsync(Organization entity);
        Task<Organization> DeleteByIDAsync(Guid id);

        //get OrganizationMember same OrganizationId
        Task<List<OrganizationMember>> GetAllOrganizationMemberByIDAsync(Expression<Func<OrganizationMember, bool>> filter);

        Task<List<OrganizationMember>> GetAllOrganizationMemberAsync();
        Task<bool> DeleteOrganizationMemberByOrganizationIDAndUserIDAsync(int organizationId, string userId);
        Task<OrganizationMember?> GetOrganizationMemberAsync(Expression<Func<OrganizationMember, bool>> filter);

        Task<bool> AddOrganizationMemberSync(OrganizationMember entity);

        Task<bool> AddOrganizationResourceSync(OrganizationResource entity);
        Task<List<OrganizationResource>> GetAllOrganizationResourceByOrganizationIDAsync(Expression<Func<OrganizationResource, bool>> filter);
        Task<OrganizationResource> GetOrganizationResourceByOrganizationIDAndResourceIDAsync(int organizationId, int resourceId);
        Task<bool> UpdateOrganizationResourceAsync(OrganizationResource entity);

        Task<bool> AddUserToOrganizationTransactionHistoryASync(UserToOrganizationTransactionHistory transactionHistory);
        Task<List<UserToOrganizationTransactionHistory>> GetAllUserToOrganizationTransactionHistoryAsync();
        Task<List<UserToOrganizationTransactionHistory>> GetAllUserToOrganizationTransactionHistoryByProcessingAsync(int organizationId);
        Task<List<UserToOrganizationTransactionHistory>> GetAllUserToOrganizationTransactionHistoryByAcceptedAsync(int organizationId);
        Task<UserToOrganizationTransactionHistory> GetUserToOrganizationTransactionHistoryByTransactionIDAsync(Expression<Func<UserToOrganizationTransactionHistory, bool>> filter);
        Task<bool> DeleteUserToOrganizationTransactionHistoryByTransactionIDAsync(int transactionID);
        Task<bool> UpdateUserToOrganizationTransactionHistoryAsync(UserToOrganizationTransactionHistory entity);

        Task<bool> AddOrganizationToProjectHistoryAsync(OrganizationToProjectHistory entity);
        Task<List<OrganizationToProjectHistory>> GetAllOrganizationToProjectHistoryAsync();
        Task<List<OrganizationToProjectHistory>> GetAllOrganizationToProjectHistoryByProcessingAsync(int organizationId);
    }
}
