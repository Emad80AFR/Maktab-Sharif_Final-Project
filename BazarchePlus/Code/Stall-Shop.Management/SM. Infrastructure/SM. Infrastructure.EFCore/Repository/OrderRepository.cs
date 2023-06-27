using AM._Infrastructure.EFCore;
using FrameWork.Application;
using FrameWork.Application.Authentication;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SM._Application.Contracts.Order.DTO_s;
using SM._Domain.OrderAgg;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SM._Infrastructure.EFCore.Repository;

public class OrderRepository:BaseRepository<long,Order>,IOrderRepository
{
    private readonly IAuthHelper _authHelper;
    private readonly ShopContext _shopContext;
    private readonly AccountContext _accountContext;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(ILogger<OrderRepository> logger, ShopContext shopContext, AccountContext accountContext, IAuthHelper authHelper):base(shopContext,logger)
    {
        _logger = logger;
        _shopContext = shopContext;
        _accountContext = accountContext;
        _authHelper = authHelper;
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
            var account = await _accountContext.Accounts.Select(x => new { x.Id, x.Fullname })
                .ToListAsync(cancellationToken: cancellationToken);
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
                UnitPrice = x.UnitPrice,
                SellerId = x.SellerId,
                WageRate = x.WageRate,
            });

            var role = _authHelper.CurrentAccountRole();
            if (role == Roles.Seller)
                items = items.Where(x => x.SellerId == _authHelper.CurrentAccountId());

            var orderItemViewModels = items.ToList();
            foreach (var item in orderItemViewModels)
            {
                item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name;
                item.SellerName = account.FirstOrDefault(x => x.Id == item.SellerId)?.Fullname;
            }

            return orderItemViewModels;
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
                IssueTrackingNo = x.IssueTrackingNo!,
                PayAmount = x.PayAmount,
                PaymentMethodId = x.PaymentMethod,
                RefId = x.RefId,
                TotalAmount = x.TotalAmount,
                CreationDate = x.CreationDate.ToFarsi(),
                WageAmount = x.WageAmount,
                Sellers = MapSellers(x.Items)
            }).AsNoTracking();

            query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

            if (searchModel.AccountId > 0)
                query = query.Where(x => x.AccountId == searchModel.AccountId);

            var orders = await query.OrderByDescending(x => x.Id).ToListAsync(cancellationToken: cancellationToken);
            foreach (var order in orders)
            {
                order.AccountFullName = accounts.FirstOrDefault(x => x.Id == order.AccountId)?.Fullname;
                order.PaymentMethod = PaymentMethod.GetBy(order.PaymentMethodId)?.Name;
            }
            var role = _authHelper.CurrentAccountRole();
            if (role == Roles.Seller)
            {
                orders = orders.Where(x => x.Sellers.Contains(_authHelper.CurrentAccountId())).ToList();
            }
            return orders;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching orders");
            throw;
        }
    }

    public async Task<bool> CheckPlaceOrder(long id, long productId, CancellationToken cancellationToken)
    {
       var orderList=await _shopContext.Orders
           .Include(x=>x.Items)
           .Where(x=>x.AccountId==id)
           .Select(x=>x.Items)
           .AsNoTracking()
           .ToListAsync(cancellationToken: cancellationToken);

        var productsId= (from order in orderList from item in order select item.ProductId).ToList();
        return productsId.Contains(productId);
    }

    private static List<long> MapSellers(IEnumerable<OrderItem> items)
    {
        return items.Select(x => x.SellerId).ToList();
    }
}