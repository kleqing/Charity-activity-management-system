﻿using System.Linq.Expressions;
using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.DataAccess.Repository;

public class UserToProjectTransactionHistoryRepository : IUserToProjectTransactionHistoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IProjectResourceRepository _projectResourceRepo;

    public UserToProjectTransactionHistoryRepository(ApplicationDbContext context, IProjectResourceRepository projectResourceRepository)
    {
        _context = context;
        _projectResourceRepo = projectResourceRepository;

    }

    public async Task<List<UserToProjectTransactionHistory>> GetAllAsyncWithExpression(
        Expression<Func<UserToProjectTransactionHistory, bool>> filter)
    {
        return await _context.UserToProjectTransactionHistories.Where(filter).Include(ut => ut.User).ToListAsync();
    }

    public Task<UserToProjectTransactionHistory?> GetAsync(
        Expression<Func<UserToProjectTransactionHistory, bool>> filter)
    {
        return _context.UserToProjectTransactionHistories.Where(filter).FirstOrDefaultAsync();
    }

    public async Task<UserToProjectTransactionHistory> DeleteTransactionByIdAsync(Guid id)
    {
        var entity = await GetAsync(tr => tr.TransactionID.Equals(id));
        if (entity == null) return null;
        var final = _context.UserToProjectTransactionHistories.Remove(entity);
        await _context.SaveChangesAsync();
        return final.Entity;
    }
    //get,add and review donate from user-huyen
    public async Task<List<UserToProjectTransactionHistory>> GetAllUserDonateAsync(
    Expression<Func<UserToProjectTransactionHistory, bool>> filter)
    {
        IQueryable<UserToProjectTransactionHistory> listUserDonate =
            _context.UserToProjectTransactionHistories.Include(x => x.ProjectResource).ThenInclude(x => x.Project).Include(x => x.User).Where(filter);
        if (listUserDonate != null)
        {
            return await listUserDonate.ToListAsync();
        }
        return null;
    }
    public async Task<bool> AddUserDonateRequestAsync(UserToProjectTransactionHistory? userDonate)
    {
        if (userDonate != null)
        {
            userDonate.TransactionID = Guid.NewGuid();
            if (userDonate.Amount <= 0)
            {
                userDonate.Amount = 1;
            }

            userDonate.Status = 0;
            userDonate.Time = DateOnly.FromDateTime(DateTime.Now);
            await _context.UserToProjectTransactionHistories.AddAsync(userDonate);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
    //------review donate request------------------
    public async Task<bool> AcceptedUserDonateRequestAsync(Guid transactionID)
    {
        var transactionObj =
            await _context.UserToProjectTransactionHistories.FirstOrDefaultAsync(x =>
                x.TransactionID.Equals(transactionID));
        if (transactionObj != null)
        {
            //change status of transaction
            transactionObj.Status = 1;
            _context.UserToProjectTransactionHistories.Update(transactionObj);
            await _context.SaveChangesAsync();

            //modify resource of project
            var addResourceAutomatic = _projectResourceRepo.HandleResourceAutomatic(transactionID, "User");
            if (addResourceAutomatic.Result)
                return true;
            return false;
        }

        return false;
    }

    public async Task<bool> DenyUserDonateRequestAsync(Guid transactionID)
    {
        var transactionObj =
            await _context.UserToProjectTransactionHistories.FirstOrDefaultAsync(x =>
                x.TransactionID.Equals(transactionID));
        if (transactionObj != null)
        {
            _context.UserToProjectTransactionHistories.Remove(transactionObj);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}