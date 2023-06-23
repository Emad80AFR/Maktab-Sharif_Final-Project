using AM._Infrastructure.EFCore;
using BP._Query.Contracts.Seller;
using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BP._Query.Query;

public class SellerQuery:ISellerQuery
{
    private readonly ILogger <SellerQuery> _logger;
    private readonly AccountContext _accountContext;

    public SellerQuery(ILogger<SellerQuery> logger, AccountContext accountContext)
    {
        _logger = logger;
        _accountContext = accountContext;
    }

    public async Task<List<SellerQueryModel>> GetSellers(CancellationToken cancellationToken)
    {
        return await _accountContext.Accounts.Where(x=>x.RoleId==long.Parse(Roles.Seller)).Select(x => new SellerQueryModel
        {
            Id = x.Id,
            Name = x.Fullname,
            ProfilePicture = x.ProfilePhoto,
            RegisterDate = x.CreationDate.ToFarsi(),
            ShopName = x.ShopName!,
            ShopPicture = x.ShopPhoto!
        }).AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
    }
}