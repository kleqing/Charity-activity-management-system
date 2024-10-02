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
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApplicationDbContext _db;

        public OrganizationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AddAsync(Organization organization)
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

        public async Task<Organization> DeleteByIDAsync(Guid id)
        {
            var organization = _db.Organizations.Find(id);
            if (organization != null)
            {
                throw new Exception("Organizaion scope standards community");
            }
            return organization;
        }

        public async Task<Organization?> GetAsync(Expression<Func<Organization, bool>> filter)
        {
            var organization = await _db.Organizations.Where(filter).FirstOrDefaultAsync();
            return organization;
        }

        public async Task<List<Organization>> GetAllOrganizationsAsync()
        {
            var organizations = await _db.Organizations.ToListAsync();
            return organizations;
        }
        public async Task<List<Organization>> GetListOrganizationsByUserIDAsync(Expression<Func<Organization, bool>> filter)
        {
            var organizations = await _db.Organizations.Where(filter).ToListAsync();
            return organizations;
        }

        public async Task<bool> UpdateAsync(Organization organization)
        {
            var organizationItem = await GetAsync(o => o.OrganizationID == organization.OrganizationID);

            if (organizationItem == null)
            {
                return false;
            }
            _db.Entry(organizationItem).CurrentValues.SetValues(organization);
            await _db.SaveChangesAsync();
            return true;

        }

        public async Task<List<OrganizationMember>> GetAllOrganizationMemberByIDAsync(Expression<Func<OrganizationMember, bool>> filter)
        {
            var organizationMembers = await _db.OrganizationMember.Where(filter).ToListAsync();
            return organizationMembers;

        }

        public async Task<List<OrganizationMember>> GetAllOrganizationMemberAsync()
        {
            var organizationMembers = await _db.OrganizationMember.ToListAsync();
            return organizationMembers;
        }

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


        //Member out or remove 
        public async Task<bool> DeleteOrganizationMemberByOrganizationIDAndUserIDAsync(int organizationId, string userId)
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
        public async Task<List<OrganizationResource>> GetAllOrganizationResourceByOrganizationIDAsync(Expression<Func<OrganizationResource, bool>> filter)
        {
            var organizationResources = await _db.OrganizationResources.Where(filter).ToListAsync();
            return organizationResources;
        }

        public async Task<OrganizationResource> GetOrganizationResourceByOrganizationIDAndResourceIDAsync(int organizationId, int resourceId)
        {
            var organizationResource = await _db.OrganizationResources.Where(or => or.OrganizationID == organizationId && or.ResourceID == resourceId).FirstOrDefaultAsync();
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

        public async Task<List<UserToOrganizationTransactionHistory>> GetAllUserToOrganizationTransactionHistoryByProcessingAsync(int organizationId)
        {
            var organizationResources = await _db.OrganizationResources.Where(or => or.OrganizationID == organizationId).ToListAsync();// list resource have ina organization
            var UserToOrganizationTransactionHistoryInAOrganizations = new List<UserToOrganizationTransactionHistory>();
            foreach (var item in await GetAllUserToOrganizationTransactionHistoryAsync())
            {
                foreach (var el in organizationResources)
                {
                    if (item.ResourceID == el.ResourceID && item.Status == 0)
                    {
                        UserToOrganizationTransactionHistoryInAOrganizations.Add(item);
                    }
                }
            }
            return UserToOrganizationTransactionHistoryInAOrganizations;
        }

        public async Task<List<UserToOrganizationTransactionHistory>> GetAllUserToOrganizationTransactionHistoryByAcceptedAsync(int organizationId)
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

        public async Task<bool> DeleteUserToOrganizationTransactionHistoryByTransactionIDAsync(int transactionID)
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
            var userToOrganizationTransactionHistoryItem = await GetUserToOrganizationTransactionHistoryByTransactionIDAsync(uto => uto.TransactionID == entity.TransactionID);

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

        public async Task<List<OrganizationToProjectHistory>> GetAllOrganizationToProjectHistoryByProcessingAsync(int organizationId)
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
