using DM._Domain.ColleagueDiscountAgg;
using DM.Application.Contracts.ColleagueDiscount.DTO_s;
using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SM._Infrastructure.EFCore;

namespace DM._Infrastructure.EFCore.Repository;

public class ColleagueDiscountRepository:BaseRepository<long,ColleagueDiscount>,IColleagueDiscountRepository
{

    private readonly ILogger<ColleagueDiscountRepository> _logger;
    private readonly DiscountContext _discountContext;
    private readonly ShopContext _shopContext;

    public ColleagueDiscountRepository(ShopContext shopContext, DiscountContext discountContext, ILogger<ColleagueDiscountRepository> logger):base(discountContext,logger)
    {
        _shopContext = shopContext;
        _discountContext = discountContext;
        _logger = logger;
    }

    public async Task<EditColleagueDiscount> GetDetails(long id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching colleague discount details...");

            var discount = await _discountContext.ColleagueDiscounts
                .Where(x => x.Id == id)
                .Select(x => new EditColleagueDiscount
                {
                    Id = x.Id,
                    DiscountRate = x.DiscountRate,
                    ProductId = x.ProductId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (discount != null)
                _logger.LogInformation("Colleague discount details retrieved successfully. Discount ID: {DiscountId}", discount.Id);
            else
                _logger.LogWarning("No colleague discount found with ID: {DiscountId}", id);

            return discount!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching colleague discount details.");
            throw; 
        }
    }

    public async Task<List<ColleagueDiscountViewModel>> Search(ColleagueDiscountSearchModel searchModel, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Performing colleague discount search...");

            var products = await _shopContext.Products
                .Select(x => new { x.Id, x.Name })
                .ToListAsync(cancellationToken);

            var query = _discountContext.ColleagueDiscounts
                .Select(x => new ColleagueDiscountViewModel
                {
                    Id = x.Id,
                    CreationDate = x.CreationDate.ToFarsi(),
                    DiscountRate = x.DiscountRate,
                    ProductId = x.ProductId,
                    IsRemoved = x.IsRemoved
                });

            if (searchModel.ProductId > 0)
                query = query.Where(x => x.ProductId == searchModel.ProductId);

            var discounts = await query.OrderByDescending(x => x.Id).ToListAsync(cancellationToken);

            discounts.ForEach(discount =>
                discount.Product = products.FirstOrDefault(x => x.Id == discount.ProductId)?.Name);

            _logger.LogInformation("Colleague discount search completed successfully. Total discounts found: {DiscountCount}", discounts.Count);

            return discounts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while performing colleague discount search.");
            throw; 
        }
    }
}