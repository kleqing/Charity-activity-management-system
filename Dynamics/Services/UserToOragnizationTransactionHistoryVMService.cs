using Dynamics.DataAccess;
using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dynamics.Services
{
    public class UserToOragnizationTransactionHistoryVMService : IUserToOragnizationTransactionHistoryVMService
    {
        private readonly ApplicationDbContext _db;

        public UserToOragnizationTransactionHistoryVMService(ApplicationDbContext db)
        {
            _db = db;
        }

        //for user request donate for Oragnization
        public async Task<List<UserToOrganizationTransactionHistory>> GetTransactionHistory(Guid organizationId)
        {
            var result = await _db.UserToOrganizationTransactionHistories.Where(uto => uto.Status == 0)
                                 .Where(uto => uto.OrganizationResource.OrganizationID.Equals(organizationId))
                                 .Include(uto => uto.User) 
                                 .Include(uto => uto.OrganizationResource)
                                        .ThenInclude(uto => uto.Organization)
          
                                  .Select(uto => new UserToOrganizationTransactionHistory
                                  {
                                      TransactionID = uto.TransactionID,
                                      ResourceID = uto.ResourceID,
                                      UserID = uto.UserID,
                                      Status = uto.Status,
                                      Unit = uto.Unit,
                                      Amount = uto.Amount,
                                      Message = uto.Message,
                                      Time = uto.Time,
                                      User = uto.User,
                                      OrganizationResource = uto.OrganizationResource,
                                  })
                                  .ToListAsync();
            return result;
        }


        //for Organization history
        public async Task<List<UserToOrganizationTransactionHistory>> GetTransactionHistoryIsAccept(Guid organizationId)
        {
            var result = await _db.UserToOrganizationTransactionHistories.Where(uto => uto.Status == 1)
                                 .Where(uto => uto.OrganizationResource.OrganizationID.Equals(organizationId))
                                 .Include(uto => uto.User)
                                 .Include(uto => uto.OrganizationResource)
                                        .ThenInclude(uto => uto.Organization)

                                  .Select(uto => new UserToOrganizationTransactionHistory
                                  {
                                      TransactionID = uto.TransactionID,
                                      ResourceID = uto.ResourceID,
                                      UserID = uto.UserID,
                                      Status = uto.Status,
                                      Unit = uto.Unit,
                                      Amount = uto.Amount,
                                      Message = uto.Message,
                                      Time = uto.Time,
                                      User = uto.User,
                                      OrganizationResource = uto.OrganizationResource,
                                  })
                                  .ToListAsync();
            return result;
        }

    }
}
