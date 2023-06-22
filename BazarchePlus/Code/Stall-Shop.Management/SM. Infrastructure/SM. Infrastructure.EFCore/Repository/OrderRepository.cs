using AM._Infrastructure.EFCore;
using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SM._Application.Contracts.Order.DTO_s;
using SM._Domain.OrderAgg;

namespace SM._Infrastructure.EFCore.Repository;

public class OrderRepository:BaseRepository<long,Order>,IOrderRepository
{
    private readonly ShopContext _shopContext;
    private readonly AccountContext _accountContext;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(ILogger<OrderRepository> logger, ShopContext shopContext, AccountContext accountContext):base(shopContext,logger)
    {
        _logger = logger;
        _shopContext = shopContext;
        _accountContext = accountContext;
    }

    public async Task<double> GetAmountBy(long id, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _shopContext.Orders
                .Select(x => new { x.PayAmount, x.Id })
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (order != null)
                return order.PayAmount;

            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting amount by ID: {Id}", id);
            throw; 
        }
    }

    public async Task<List<OrderItemViewModel>> GetItems(long orderId, CancellationToken cancellationToken)
    {
        try
        {
            var products = await _shopContext.Products.Select(x => new { x.Id, x.Name }).ToListAsync(cancellationToken: cancellationToken);
            var order = await _shopContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken: cancellationToken);

            if (order == null)
                return new List<OrderItemViewModel>();

            var items = order.Items.Select(x => new OrderItemViewModel
            {
                Id = x.Id,
                Count = x.Count,
                DiscountRate = x.DiscountRate,
                OrderId = x.OrderId,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice
            }).ToList();

            foreach (var item in items)
            {
                item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name;
            }

            return items;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting items for order ID: {OrderId}", orderId);
            throw; 
        }
    }

    public async Task<List<OrderViewModel>> Search(OrderSearchModel searchModel, CancellationToken cancellationToken)
    {
        try
        {
            var accounts = await _accountContext.Accounts.Select(x => new { x.Id, x.Fullname })
                .ToListAsync(cancellationToken: cancellationToken);
            var query = _shopContext.Orders.Select(x => new OrderViewModel
            {
                Id = x.Id,
                AccountId = x.AccountId,
                DiscountAmount = x.DiscountAmount,
                IsCanceled = x.IsCanceled,
                IsPaid = x.IsPaid,
                IssueTrackingNo = x.IssueTrackingNo,
                PayAmount = x.PayAmount,
                PaymentMethodId = x.PaymentMethod,
                RefId = x.RefId,
                TotalAmount = x.TotalAmount,
                CreationDate = x.CreationDate.ToFarsi()
            });

            query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

            if (searchModel.AccountId > 0)
                query = query.Where(x => x.AccountId == searchModel.AccountId);

            var orders = await query.OrderByDescending(x => x.Id).ToListAsync(cancellationToken: cancellationToken);
            foreach (var order in orders)
            {
                order.AccountFullName = accounts.FirstOrDefault(x => x.Id == order.AccountId)?.Fullname;
                order.PaymentMethod = PaymentMethod.GetBy(order.PaymentMethodId)?.Name;
            }

            return orders;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching orders");
            throw;
        }
    }
}