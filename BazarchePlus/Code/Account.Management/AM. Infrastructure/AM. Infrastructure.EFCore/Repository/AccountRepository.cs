﻿using AM._Application.Contracts.Account.DTO_s;
using AM._Domain.AccountAgg;
using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AM._Infrastructure.EFCore.Repository;

public class AccountRepository:BaseRepository<long,Account>, IAccountRepository
{
    private readonly ILogger<AccountRepository> _logger;
    private readonly AccountContext _context;
    public AccountRepository(AccountContext context, ILogger<AccountRepository> logger) : base(context, logger)
    {
        _context = context;
        _logger=logger;
    }

    public async Task<Account> GetBy(string username, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Username == username, cancellationToken: cancellationToken);

        if (account != null)
        {
            _logger.LogInformation("Account found successfully");
        }
        else
        {
            _logger.LogWarning("Account not found: {Username}", username);
        }

        return account!;
    }

    public async Task<EditAccount> GetDetails(long id, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .Select(x => new EditAccount
            {
                Id = x.Id,
                Fullname = x.Fullname,
                Mobile = x.Mobile,
                RoleId = x.RoleId,
                Username = x.Username,
                ProfilePictureName = x.ProfilePhoto,
                ShopPictureName = x.ShopPhoto,
                ShopName = x.ShopName
                
            })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (account != null)
        {
            _logger.LogInformation("Account found successfully");
        }
        else
        {
            _logger.LogWarning("Account not found: {Id}", id);
        }

        return account!;
    }

    public async Task<List<AccountViewModel>> GetAccounts(CancellationToken cancellationToken)
    {
        var accounts = await _context.Accounts
            .Select(x => new AccountViewModel
            {
                Id = x.Id,
                Fullname = x.Fullname
            })
            .ToListAsync(cancellationToken: cancellationToken);

        _logger.LogInformation("Retrieved account list successfully");

        return accounts;
    }

    public async Task<List<AccountViewModel>> Search(AccountSearchModel searchModel, CancellationToken cancellationToken)
    {
        var query = _context.Accounts
            .Include(x => x.Role)
            .Select(x => new AccountViewModel
            {
                Id = x.Id,
                Fullname = x.Fullname,
                Mobile = x.Mobile,
                ProfilePhoto = x.ProfilePhoto,
                Role = x.Role.Name,
                RoleId = x.RoleId,
                Username = x.Username,
                CreationDate = x.CreationDate.ToFarsi(),
                IsActive = x.IsActive
                
            });

        if (searchModel.IsActive)
            query = query.Where(x => !x.IsActive);

        if (!string.IsNullOrWhiteSpace(searchModel.Fullname))
            query = query.Where(x => x.Fullname.Contains(searchModel.Fullname));

        if (!string.IsNullOrWhiteSpace(searchModel.Username))
            query = query.Where(x => x.Username.Contains(searchModel.Username));

        if (!string.IsNullOrWhiteSpace(searchModel.Mobile))
            query = query.Where(x => x.Mobile.Contains(searchModel.Mobile));

        if (searchModel.RoleId > 0)
            query = query.Where(x => x.RoleId == searchModel.RoleId);

        var accounts = await query.OrderByDescending(x => x.Id).ToListAsync(cancellationToken: cancellationToken);

        _logger.LogInformation("Retrieved account list successfully");

        return accounts;
    }

    public async Task<FinancialModel> GetFinancialInfo(long id, CancellationToken cancellationToken)
    {
       return (await _context.Accounts.Select(x => new FinancialModel
       {
           Id = x.Id,
           Medal =x.Medal,
           SaleAmount = x.SalesAmount
       }).FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken))!;
    }

    public Task<Account> GetManagerAccount(CancellationToken cancellationToken)
    {
       return _context.Accounts.FirstOrDefaultAsync(x => x.RoleId == int.Parse(Roles.Developer), cancellationToken)!;
    }

    public async Task<string> GetAccountName(long id, CancellationToken cancellationToken)
    {
        return (await _context.Accounts
            .Where(x => x.Id == id)
            .Select(x => x.Fullname)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken))!;
    }
}