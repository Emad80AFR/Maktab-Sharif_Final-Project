using System.Security.Cryptography.X509Certificates;
using FrameWork.Application;
using FrameWork.Application.Authentication;
using FrameWork.Infrastructure.ConfigurationModel;
using Microsoft.Extensions.Logging;
using SM._Application.Contracts.Order;
using SM._Application.Contracts.Order.DTO_s;
using SM._Domain.OrderAgg;
using SM._Domain.Services;

namespace SM._Application.Implementation;

public class OrderApplication:IOrderApplication
{
    private readonly IAuthHelper _authHelper;
    private readonly IShopAccountAcl _shopAccountAcl;
    private readonly ILogger<OrderApplication> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IShopInventoryAcl _shopInventoryAcl;
    private readonly AppSettingsOption.Domainsettings _appOptions;
    public OrderApplication(ILogger<OrderApplication> logger, IOrderRepository orderRepository, IAuthHelper authHelper, AppSettingsOption.Domainsettings appOptions, IShopInventoryAcl shopInventoryAcl, IShopAccountAcl shopAccountAcl)
    {
        _logger = logger;
        _authHelper = authHelper;
        _appOptions = appOptions;
        _orderRepository = orderRepository;
        _shopInventoryAcl = shopInventoryAcl;
        _shopAccountAcl = shopAccountAcl;
    }

    public async Task<long> PlaceOrder(Cart cart, CancellationToken cancellationToken)
    {
        try
        {
            var currentAccountId = _authHelper.CurrentAccountId();
            var order = new Order(currentAccountId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount, cart.PayAmount,cart.WageAmount);

            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem(cartItem.Id, cartItem.Count, cartItem.UnitPrice, cartItem.DiscountRate,cartItem.WageRate,cartItem.SellerId);
                order.AddItem(orderItem);
            }

            await _orderRepository.Create(order,cancellationToken);
            await _orderRepository.SaveChanges(cancellationToken);
            return order.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while placing an order");
            throw; 
        }
    }

    public async Task<double> GetAmountBy(long id, CancellationToken cancellationToken)
    {
        try
        {
            return await _orderRepository.GetAmountBy(id,cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving amount for order with ID: {id}");
            throw; 
        }
    }

    public async Task Cancel(long id, CancellationToken cancellationToken)
    {

        try
        {
            var order = await _orderRepository.Get(id,cancellationToken);
            order?.Cancel();
            await _orderRepository.SaveChanges(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while canceling order with ID: {id}");
            throw; 
        }
    }

    public async Task<string> PaymentSucceeded(long orderId, long refId, CancellationToken cancellationToken)
    {
        try
        {
            var order =await _orderRepository.Get(orderId,cancellationToken);
            order!.PaymentSucceeded(refId);
            var symbol = _appOptions.CodeGenerator.PreFixCode;
            var issueTrackingNo = CodeGenerator.Generate(symbol);
            order.SetIssueTrackingNo(issueTrackingNo);
            if ( !await _shopInventoryAcl.ReduceFromInventory(order.Items,cancellationToken))
            {
                _logger.LogWarning($"Failed to reduce inventory for order with ID: {orderId}");
                return "";
            }


            order.Items.ForEach(item =>
            {
                _shopAccountAcl.UpdateFinancialInfo(item, cancellationToken);
                _shopAccountAcl.CalculateSaleAmount(item, cancellationToken);
            });
            //async void Action(OrderItem x)
            //{
            //    await _shopAccountAcl.UpdateFinancialInfo(x, cancellationToken);
            //    //await _shopAccountAcl.CalculateSaleAmount(x.SellerId, cancellationToken);
            //}

            //order.Items.ForEach(Action);

            await _orderRepository.SaveChanges(cancellationToken);

            //var (name, mobile) = _shopAccountAcl.GetAccountBy(order.AccountId);

            //_smsService.Send(mobile,
            //    $"{name}, your order with tracking number {issueTrackingNo} has been successfully paid and will be shipped.");

            return issueTrackingNo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while processing payment for order with ID: {orderId}");
            throw; 
        }
    }

    public async Task<List<OrderItemViewModel>> GetItems(long orderId, CancellationToken cancellationToken)
    {
        try
        {
            return await _orderRepository.GetItems(orderId,cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving items for order with ID: {orderId}");
            throw; 
        }
    }

    public async Task<List<OrderViewModel>> Search(OrderSearchModel searchModel, CancellationToken cancellationToken)
    {
        try
        {
            return await _orderRepository.Search(searchModel,cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while searching orders with criteria: {searchModel}");
            throw; 
        }
    }
}