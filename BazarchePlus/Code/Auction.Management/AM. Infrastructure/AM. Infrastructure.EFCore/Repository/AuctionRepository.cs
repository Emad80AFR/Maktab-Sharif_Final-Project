using System.Globalization;
using AM._Application.Contracts.Auction.DTO_s;
using AM._Domain.AuctionAgg;
using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SM._Infrastructure.EFCore;

namespace AM._Infrastructure.EFCore.Repository;

public class AuctionRepository:BaseRepository<long,Auction>,IAuctionRepository
{
    private readonly AuctionContext _auctionContext;
    private readonly ShopContext _shopContext;
    private readonly ILogger<AuctionRepository> _logger;

    public AuctionRepository(AuctionContext auctionContext, ILogger<AuctionRepository> logger, ShopContext shopContext):base(auctionContext,logger)
    {
        _auctionContext = auctionContext;
        _logger = logger;
        _shopContext = shopContext;
    }

    public async Task<EditAuction> GetDetails(long id, CancellationToken cancellationToken)
    {
        try
        {
            var auction = await _auctionContext.Auctions
                .Select(x => new EditAuction
                {
                    Id = x.Id,
                    BasePrice = x.BasePrice,
                    EndDate = x.EndDate.ToString(CultureInfo.InvariantCulture),
                    ProductId = x.ProductId
                })
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (auction == null)
            {
                _logger.LogWarning("Auction with ID {AuctionId} not found", id);
            }
            else
            {
                _logger.LogInformation("Retrieved auction details for ID {AuctionId}", id);
            }

            return auction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting auction details for ID {AuctionId}", id);
            throw;
        }
    }

    public async Task<List<AuctionViewModel>> Search(AuctionSearchModel searchModel, CancellationToken cancellationToken)
    {
        var products = await _shopContext.Products
            .Select(x => new { x.Id, x.Name })
            .ToListAsync(cancellationToken: cancellationToken);

        var query = _auctionContext.Auctions
            .Select(x => new AuctionViewModel
            {
                Id = x.Id,
                Status = x.Status,
                BasePrice = x.BasePrice,
                EndDate = x.EndDate.ToFarsi(),
                IsActive = x.IsActive,
                ProductId = x.ProductId,
                CreationDate = x.CreationDate.ToFarsi()
            });

        if (searchModel.ProductId > 0)
        {
            query = query.Where(x => x.ProductId == searchModel.ProductId);
        }

        if (searchModel.IsActive)
        {
            query = query.Where(x => !x.IsActive);
        }

        try
        {
            var auctions = await query.OrderByDescending(x => x.Id).ToListAsync(cancellationToken: cancellationToken);

            foreach (var auction in auctions)
            {
                var product = products.FirstOrDefault(p => p.Id == auction.ProductId);
                if (product != null)
                {
                    auction.ProductName = product.Name;
                }
                else
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for auction with ID {AuctionId}", auction.ProductId, auction.Id);
                }
            }

            _logger.LogInformation("Retrieved auctions successfully");

            return auctions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving auctions");
            throw; 
        }
    }
}