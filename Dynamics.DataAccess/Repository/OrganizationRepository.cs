using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.DataAccess.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApplicationDbContext _db;

        public OrganizationRepository(ApplicationDbContext db)
        {
            _db = db;
        }


        //Organization table

        public async Task<bool> AddOrganizationAsync(Organization organization)
        {
            try
            {
                _db.Organizations.Add(organization);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<Organization?> GetOrganizationAsync(Expression<Func<Organization, bool>> filter)
        {
            var organization = await _db.Organizations.Where(filter).FirstOrDefaultAsync();
            return organization;
        }
        //public async Task<List<Organization>> GetListOrganizationsByUserIDAsync(Expression<Func<Organization, bool>> filter)
        //{
        //    var organizations = await _db.Organizations.Where(filter).ToListAsync();
        //    return organizations;
        //}

        public async Task<bool> UpdateOrganizationAsync(Organization organization)
        {
            var organizationItem = await GetOrganizationAsync(o => o.OrganizationID == organization.OrganizationID);

            if (organizationItem == null)
            {
                return false;
            }
            _db.Entry(organizationItem).CurrentValues.SetValues(organization);
            await _db.SaveChangesAsync();
            return true;

        }


        //Organization Member table
        //Member join organization
        public async Task<bool> AddOrganizationMemberSync(OrganizationMember organizationMember)
        {
            try
            {
                _db.OrganizationMember.Add(organizationMember);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<OrganizationMember> GetOrganizationMemberAsync(Expression<Func<OrganizationMember, bool>> filter)
        {
            var organizationMember = await _db.OrganizationMember.Where(filter).FirstOrDefaultAsync();
            return organizationMember;
        }

        public async Task<bool> UpdateOrganizationMemberAsync(OrganizationMember entity)
        {
            var organizationItem = await GetOrganizationMemberAsync(om => om.OrganizationID == entity.OrganizationID && om.UserID == entity.UserID);

            if (organizationItem == null)
            {
                return false;
            }
            _db.Entry(organizationItem).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        //Member out or remove 
        public async Task<bool> DeleteOrganizationMemberByOrganizationIDAndUserIDAsync(Guid organizationId, Guid userId)
        {
            var organizationMember = await GetOrganizationMemberAsync(om => om.OrganizationID == organizationId && om.UserID == userId);
            try
            {
                _db.OrganizationMember.Remove(organizationMember);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }


        //Organization Resource
        public async Task<bool> AddOrganizationResourceSync(OrganizationResource organizationResource)
        {
            try
            {
                _db.OrganizationResources.Add(organizationResource);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<OrganizationResource> GetOrganizationResourceByOrganizationIDAndResourceIDAsync(Guid organizationId, Guid resourceId)
        {
            var organizationResource = await _db.OrganizationResources.Where(or => or.OrganizationID == organizationId && or.ResourceID == resourceId).FirstOrDefaultAsync();
            return organizationResource;
        }

        public async Task<OrganizationResource> GetOrganizationResourceAsync(Expression<Func<OrganizationResource, bool>> filter)
        {
            var organizationResource = await _db.OrganizationResources.Where(filter).FirstOrDefaultAsync();
            return organizationResource;
        }

        public async Task<bool> UpdateOrganizationResourceAsync(OrganizationResource entity)
        {
            var organizationResourceItem = await GetOrganizationResourceByOrganizationIDAndResourceIDAsync(entity.OrganizationID, entity.ResourceID);

            if (organizationResourceItem == null)
            {
                return false;
            }
            _db.Entry(organizationResourceItem).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrganizationResourceAsync(Guid resourceId)
        {
            var organizationResource = await GetOrganizationResourceAsync(om => om.ResourceID.Equals(resourceId));
            try
            {
                _db.OrganizationResources.Remove(organizationResource);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }





        public async Task<bool> AddUserToOrganizationTransactionHistoryASync(UserToOrganizationTransactionHistory transactionHistory)
        {
            try
            {
                _db.UserToOrganizationTransactionHistories.Add(transactionHistory);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<UserToOrganizationTransactionHistory>> GetAllUserToOrganizationTransactionHistoryAsync()
        {
            var uerToOrganizationTransactionHistorys = await _db.UserToOrganizationTransactionHistories.ToListAsync();
            return uerToOrganizationTransactionHistorys;
        }


        public async Task<List<UserToOrganizationTransactionHistory>> GetAllUserToOrganizationTransactionHistoryByAcceptedAsync(Guid organizationId)
        {
            var organizationResources = await _db.OrganizationResources.Where(or => or.OrganizationID == organizationId).ToListAsync();// list resource have ina organization
            var UserToOrganizationTransactionHistoryInAOrganizations = new List<UserToOrganizationTransactionHistory>();
            foreach (var item in await GetAllUserToOrganizationTransactionHistoryAsync())
            {
                foreach (var el in organizationResources)
                {
                    if (item.ResourceID == el.ResourceID && item.Status == 1)
                    {
                        UserToOrganizationTransactionHistoryInAOrganizations.Add(item);
                    }
                }
            }
            return UserToOrganizationTransactionHistoryInAOrganizations;
        }

        public async Task<UserToOrganizationTransactionHistory> GetUserToOrganizationTransactionHistoryByTransactionIDAsync(Expression<Func<UserToOrganizationTransactionHistory, bool>> filter)
        {
            var userToOrganizationTransactionHistory = await _db.UserToOrganizationTransactionHistories.Where(filter).FirstOrDefaultAsync();
            return userToOrganizationTransactionHistory;
        }

        public async Task<bool> DeleteUserToOrganizationTransactionHistoryByTransactionIDAsync(Guid transactionID)
        {
            var userToOrganizationTransactionHistory = await GetUserToOrganizationTransactionHistoryByTransactionIDAsync(uto => uto.TransactionID == transactionID);
            try
            {
                _db.UserToOrganizationTransactionHistories.Remove(userToOrganizationTransactionHistory);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserToOrganizationTransactionHistoryAsync(UserToOrganizationTransactionHistory entity)
        {
            var userToOrganizationTransactionHistoryItem = await GetUserToOrganizationTransactionHistoryByTransactionIDAsync(uto => uto.TransactionID.Equals(entity.TransactionID));

            if (userToOrganizationTransactionHistoryItem == null)
            {
                return false;
            }
            _db.Entry(userToOrganizationTransactionHistoryItem).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddOrganizationToProjectHistoryAsync(OrganizationToProjectHistory entity)
        {
            try
            {
                _db.OrganizationToProjectTransactionHistory.Add(entity);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<OrganizationToProjectHistory>> GetAllOrganizationToProjectHistoryAsync()
        {
            var organizationToProjectHistorys = await _db.OrganizationToProjectTransactionHistory.ToListAsync();
            return organizationToProjectHistorys;
        }

        public async Task<List<OrganizationToProjectHistory>> GetAllOrganizationToProjectHistoryByProcessingAsync(Guid organizationId)
        {
            var organizationResources = await _db.OrganizationResources.Where(or => or.OrganizationID == organizationId).ToListAsync();// list resource have ina organization
            var organizationToProjectHistoryInAOrganizations  = new List<OrganizationToProjectHistory>();
            foreach(var item in await GetAllOrganizationToProjectHistoryAsync())
            {
                foreach(var or in organizationResources)
                {
                    if(item.OrganizationResourceID == or.ResourceID && item.Status == 0)
                    {
                        organizationToProjectHistoryInAOrganizations.Add(item);
                    }
                }
            }
            return organizationToProjectHistoryInAOrganizations;

        }


    }
}
