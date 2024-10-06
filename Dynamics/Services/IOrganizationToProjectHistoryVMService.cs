using Dynamics.Models.Models;

namespace Dynamics.Services
{
    public interface IOrganizationToProjectHistoryVMService
    {

        Task<List<OrganizationToProjectHistory>> GetAllOrganizationToProjectHistoryByPendingAsync(Guid organizationId);
    }
}
