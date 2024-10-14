using System.Linq.Expressions;
using Dynamics.Models.Models;
using Dynamics.Models.Models.DTO;
using Dynamics.Models.Models.ViewModel;

namespace Dynamics.Services;

public interface ITransactionViewService
{
    /**
     * Get the transaction entity in dto for display
     */
    Task<List<UserTransactionDto>> GetUserToOrganizationTransactionDTOsAsync(
        Expression<Func<UserToOrganizationTransactionHistory, bool>> predicate);

    /**
    * Get the transaction entity in dto for display
    */
    Task<List<UserTransactionDto>> GetUserToProjectTransactionDTOsAsync(
        Expression<Func<UserToProjectTransactionHistory, bool>> predicate);
}