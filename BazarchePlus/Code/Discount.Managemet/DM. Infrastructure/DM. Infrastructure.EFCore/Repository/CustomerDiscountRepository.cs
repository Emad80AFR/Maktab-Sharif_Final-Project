using DM._Domain.CustomerDiscountAgg;
using DM.Application.Contracts.CustomerDiscount.DTO_s;
using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SM._Infrastructure.EFCore;
using System.Globalization;

namespace DM._Infrastructure.EFCore.Repository;

public class CustomerDiscountRepository:BaseRepository<long,CustomerDiscount>,ICustomerDiscountRepository
{
    private readonly DiscountContext _discountContext;
    private readonly ShopContext _shopContext;
    private readonly ILogger<CustomerDiscountRepository> _logger;

    public CustomerDiscountRepository(ILogger<CustomerDiscountRepository> logger, ShopContext shopContext, DiscountContext discountContext):base(discountContext,logger)
    {
        _logger = logger;
        _shopContext = shopContext;
        _discountContext = discountContext;
    }

    public async Task<EditCustomerDiscount> GetDetails(long id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching customer discount details...");

            var customerDiscount = await _discountContext.CustomerDiscounts
                .Where(x => x.Id == id)
                .Select(x => new EditCustomerDiscount
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    DiscountRate = x.DiscountRate,
                    StartDate = x.StartDate.ToString(CultureInfo.InvariantCulture),
                    EndDate = x.EndDate.ToString(CultureInfo.InvariantCulture),
                    Reason = x.Reason
                })
                .FirstOrDefaultAsync(cancellationToken);

            //  if the customer discount details were retrieved
            if (customerDiscount != null)
                _logger.LogInformation("Customer discount details retrieved successfully. CustomerDiscountId: {CustomerDiscountId}", customerDiscount.Id);
            else
                _logger.LogWarning("No customer discount details found.");

            return customerDiscount!;
        }
        catch (Exception ex)
        {
            // Log the error message
            _logger.LogError(ex, "An error occurred while fetching customer discount details.");
            throw; 
        }
    }

    public async Task<List<CustomerDiscountViewModel>> Search(CustomerDiscountSearchModel searchModel, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Performing customer discount search...");

            var products = await _shopContext.Products.Select(x => new { x.Id, x.Name }).ToListAsync(cancellationToken);

            var query = _discountContext.CustomerDiscounts.Select(x => new CustomerDiscountViewModel
            {
                Id = x.Id,
                DiscountRate = x.DiscountRate,
                EndDate = x.EndDate.ToFarsi(),
                EndDateGr = x.EndDate,
                StartDate = x.StartDate.ToFarsi(),
                StartDateGr = x.StartDate,
                ProductId = x.ProductId,
                Reason = x.Reason,
                CreationDate = x.CreationDate.ToFarsi()
            });

            if (searchModel.ProductId > 0)
                query = query.Where(x => x.ProductId == searchModel.ProductId);

            if (!string.IsNullOrWhiteSpace(searchModel.StartDate))
            {
                query = query.Where(x => x.StartDateGr >= searchModel.StartDate.ToGeorgianDateTime());
            }

            if (!string.IsNullOrWhiteSpace(searchModel.EndDate))
            {
                query = query.Where(x => x.EndDateGr <= searchModel.EndDate.ToGeorgianDateTime());
            }

            var discounts = await query.OrderByDescending(x => x.Id).ToListAsync(cancellationToken);

            discounts.ForEach(discount =>
                discount.Product = products.FirstOrDefault(x => x.Id == discount.ProductId)?.Name!);

            // if the customer discounts were retrieved
            if (discounts != null && discounts.Any())
                _logger.LogInformation("Customer discounts retrieved successfully. Total discounts: {DiscountCount}", discounts.Count);
            else
                _logger.LogWarning("No customer discounts found.");

            return discounts;
        }
        catch (Exception ex)
        {
            // Log the error message
            _logger.LogError(ex, "An error occurred while performing customer discount search.");
            throw; 
        }
    }
}