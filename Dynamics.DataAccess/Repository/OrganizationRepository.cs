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
        
        public async Task<List<Organization>> GetAllOrganizationsAsync(string? includeObjects = null)
        {
            IQueryable<Organization> organizations = _db.Organizations;
            if (!string.IsNullOrEmpty(includeObjects))
            {
                foreach (var includeObj in includeObjects.Split(
                    new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    organizations = organizations.Include(includeObj);
                }
            }
            return await organizations.ToListAsync();
        }
        public IQueryable<Organization> GetAll()
        {
            return _db.Organizations.Include(o => o.OrganizationMember).ThenInclude(om => om.User);
        }

        public async Task<List<Organization>> GetAllOrganizationsWithExpressionAsync(Expression<Func<Organization, bool>>? filter = null)
        {
            if (filter != null)
            {
                return await _db.Organizations
                    .Where(filter)
                    .Include(o => o.OrganizationMember).ThenInclude(om => om.User).ToListAsync();
            }
            return await _db.Organizations.Include(o => o.OrganizationMember).ThenInclude(om => om.User).ToListAsync();
        }

        public async Task<Organization> GetOrganizationOfAUser(Guid userId)
        {
            var OrganizationObj = _db.OrganizationMember.Where(x=>x.UserID.Equals(userId)&&x.Status==2).Include("Organization").FirstOrDefaultAsync().Result;
            if (OrganizationObj != null)
            {
                return OrganizationObj.Organization;
            }
            return null;
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
        
    }

}
