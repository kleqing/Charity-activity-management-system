using Dynamics.DataAccess;
using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.Services
{
    public class OrganizationToProjectHistoryVMService : IOrganizationToProjectHistoryVMService
    {

        private readonly ApplicationDbContext _db;

        public OrganizationToProjectHistoryVMService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<OrganizationToProjectHistory>> GetAllOrganizationToProjectHistoryByPendingAsync(Guid organizationId)
        {
            var result = await _db.OrganizationToProjectTransactionHistory.Where(otp => otp.Status == 0)
                                  .Where(otp => otp.OrganizationResource.OrganizationID.Equals(organizationId))
                                  .Include(otp => otp.OrganizationResource)
                                  .Include(otp => otp.ProjectResource)
                                           .ThenInclude(pr => pr.Project)
                                   .Select(otp => new OrganizationToProjectHistory
                                   {
                                       TransactionID = otp.TransactionID,
                                       OrganizationResourceID = otp.OrganizationResourceID,
                                       ProjectResourceID = otp.ProjectResourceID,
                                       Status = otp.Status,
                                       Time = otp.Time,
                                       Amount = otp.Amount,
                                       OrganizationResource = otp.OrganizationResource,
                                       ProjectResource = otp.ProjectResource,

                                   })
                                   .ToListAsync();
            return result;
        }


    }
}
