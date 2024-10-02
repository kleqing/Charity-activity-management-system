using System.Linq.Expressions;
using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;

namespace Dynamics.Services;

public interface ITransactionViewService
{
    Task<List<UserTransactionDto>> GetUserToOrganizationTransactionDTOsAsync(Expression<Func<UserToOrganizationTransactionHistory, bool>> predicate);
    Task<List<UserTransactionDto>> GetUserToProjectTransactionDTOsAsync(Expression<Func<UserToProjectTransactionHistory, bool>> predicate);
}