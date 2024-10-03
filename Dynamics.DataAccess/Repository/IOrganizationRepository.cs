using Dynamics.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.DataAccess.Repository
{
    public interface IOrganizationRepository
    {
        Task<List<Organization>> GetAllOrganizationsAsync(string? includeObjects = null);
        IQueryable<Organization> GetAll();
        Task<Organization> GetOrganizationOfAUser(Guid userId);
        Task<Organization> GetOrganizationUserLead(Guid userId);
    }
}
