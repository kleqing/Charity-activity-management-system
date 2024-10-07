using Dynamics.DataAccess;
using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models.DTO;

namespace Dynamics.Services;

public class TransactionViewService : ITransactionViewService
{
    private readonly ApplicationDbContext _context;
    private readonly IUserToOrganizationTransactionHistoryRepository _userToOrgRepo;

    public TransactionViewService(ApplicationDbContext context, IUserToOrganizationTransactionHistoryRepository userToOrgRepo)
    {
        _context = context;
        _userToOrgRepo = userToOrgRepo;
    }

    public async Task<List<UserTransactionDto>> GetUserToOrganizationTransactionDTOsAsync(Expression<Func<UserToOrganizationTransactionHistory, bool>> filter)
    {
        var result = _context.UserToOrganizationTransactionHistories
            .Where(filter)
            .Select(ut => new UserTransactionDto
            {
                TransactionID = ut.TransactionID,
                User = ut.User,
                Amount = ut.Amount,
                Message = ut.Message,
                Status = ut.Status,
                ResourceName = ut.OrganizationResource.ResourceName,
                Target = "Organization - " + ut.OrganizationResource.Organization.OrganizationName,
                Time = ut.Time,
                Unit = ut.OrganizationResource.Unit,
                Type = "organization"
            });
        return await result.ToListAsync();
    }

    public Task<List<UserTransactionDto>> GetUserToProjectTransactionDTOsAsync(Expression<Func<UserToProjectTransactionHistory, bool>> predicate)
    {
        var result = _context.UserToProjectTransactionHistories
            .Where(predicate)
            .Select(ut => new UserTransactionDto
            {
                TransactionID = ut.TransactionID,
                User = ut.User,
                Amount = ut.Amount,
                Message = ut.Message,
                Status = ut.Status,
                ResourceName = ut.ProjectResource.ResourceName,
                Target = "Project - " + ut.ProjectResource.Project.ProjectName,
                Time = ut.Time,
                Unit = ut.ProjectResource.Unit,
                Type = "project"
            });
        return result.ToListAsync();
    }
}