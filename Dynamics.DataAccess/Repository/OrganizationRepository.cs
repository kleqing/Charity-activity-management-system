using System.Linq.Expressions;
using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.DataAccess.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApplicationDbContext _db;

        public OrganizationRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        
        public async Task<OrganizationResource?> GetAsync(Expression<Func<OrganizationResource, bool>> predicate)
        {
            return await _db.OrganizationResources.FirstOrDefaultAsync(predicate);
        }
        
        public async Task<List<Organization>> GetAllOrganizationsAsync()
        {
            IQueryable<Organization> organizations = _db.Organizations.Include(x => x.OrganizationMember).ThenInclude(x => x.User).Include(x => x.OrganizationResource);
            return await organizations.ToListAsync();
        }
        public IQueryable<Organization> GetAll()
        {
            return _db.Organizations;
        }

        public async Task<Organization> GetOrganizationUserLead(Guid userId)
        {
            var organizationMemberOfUser = _db.OrganizationMember.Where(x=>x.UserID.Equals(userId)&&x.Status==2);
            if (organizationMemberOfUser == null)
            {
                return null;
            }
            var organizationUserLead = await organizationMemberOfUser.Include("Organization").FirstOrDefaultAsync();
            if (organizationUserLead == null)
            {
                return null;
            }
            return organizationUserLead.Organization;
        }
        public async Task<Guid> GetOrgResourceIDCorresponding(Guid projectResourceID, Guid organizationUserLeadID)
        {
            var projectResourceObj = await _db.ProjectResources.FirstOrDefaultAsync(x=>x.ResourceID.Equals(projectResourceID));
            if (projectResourceObj == null)
            {
                return Guid.Empty;
            }
            var orgResourceObjCorresponding = await _db.OrganizationResources.FirstOrDefaultAsync(
                x => x.OrganizationID.Equals(organizationUserLeadID) 
                && x.ResourceName.ToLower().Trim().Equals(projectResourceObj.ResourceName.ToLower().Trim()) 
                && x.Unit.ToLower().Trim().Equals(projectResourceObj.Unit.ToLower().Trim()));
            if (orgResourceObjCorresponding == null)
            {
                return Guid.Empty;

            }
            return orgResourceObjCorresponding.ResourceID;
        }



    }

}
