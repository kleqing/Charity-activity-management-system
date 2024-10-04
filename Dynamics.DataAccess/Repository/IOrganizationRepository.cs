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
        //Organization Table
        Task<Organization?> GetOrganizationAsync(Expression<Func<Organization, bool>> filter);
        //Task<List<Organization>> GetListOrganizationsByUserIDAsync(Expression<Func<Organization, bool>> filter);
        Task<bool> AddOrganizationAsync(Organization entity);
        Task<bool> UpdateOrganizationAsync(Organization entity);

        //Organization Member 
        //get OrganizationMember same OrganizationId
        Task<bool> DeleteOrganizationMemberByOrganizationIDAndUserIDAsync(Guid organizationId, Guid userId);
        Task<OrganizationMember?> GetOrganizationMemberAsync(Expression<Func<OrganizationMember, bool>> filter);
        Task<bool> AddOrganizationMemberSync(OrganizationMember entity);
        Task<bool> UpdateOrganizationMemberAsync(OrganizationMember entity);


        //Organization Resource
        Task<bool> AddOrganizationResourceSync(OrganizationResource entity);
        Task<OrganizationResource> GetOrganizationResourceByOrganizationIDAndResourceIDAsync(Guid organizationId, Guid resourceId);
        Task<OrganizationResource> GetOrganizationResourceAsync(Expression<Func<OrganizationResource, bool>> filter);
        Task<bool> UpdateOrganizationResourceAsync(OrganizationResource entity);
        Task<bool> DeleteOrganizationResourceAsync(Guid resourceId);


        //UserToOrganizationTransactionHistory Table
        Task<bool> AddUserToOrganizationTransactionHistoryASync(UserToOrganizationTransactionHistory transactionHistory);
        Task<List<UserToOrganizationTransactionHistory>> GetAllUserToOrganizationTransactionHistoryAsync();
        Task<List<UserToOrganizationTransactionHistory>> GetAllUserToOrganizationTransactionHistoryByAcceptedAsync(Guid organizationId);
        Task<UserToOrganizationTransactionHistory> GetUserToOrganizationTransactionHistoryByTransactionIDAsync(Expression<Func<UserToOrganizationTransactionHistory, bool>> filter);
        Task<bool> DeleteUserToOrganizationTransactionHistoryByTransactionIDAsync(Guid transactionID);
        Task<bool> UpdateUserToOrganizationTransactionHistoryAsync(UserToOrganizationTransactionHistory entity);



        //OrganizationToProjectHistory table
        Task<bool> AddOrganizationToProjectHistoryAsync(OrganizationToProjectHistory entity);
        Task<List<OrganizationToProjectHistory>> GetAllOrganizationToProjectHistoryAsync();
        Task<List<OrganizationToProjectHistory>> GetAllOrganizationToProjectHistoryByProcessingAsync(Guid organizationId);
    }
}
