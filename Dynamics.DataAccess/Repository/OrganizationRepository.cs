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



    }
}
