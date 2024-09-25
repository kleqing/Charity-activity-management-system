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
        Task<List<OrganizationMember>> GetAllOrganizationMemberByOrganizationIDAsync(Expression<Func<OrganizationMember, bool>> filter);


    }
}
