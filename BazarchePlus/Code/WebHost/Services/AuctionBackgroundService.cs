using AM._Application.Contracts.Account;
using AM._Application.Contracts.Auction;
using SM._Application.Contracts.Order;
using SM._Application.Contracts.Order.DTO_s;
using SM._Application.Contracts.Product;

namespace WebHost.Services;

public class AuctionBackgroundService : BackgroundService
{
    private readonly ICalculateWage _calculateWage;
    private readonly IAuctionService _auctionService;
    private readonly IOrderApplication _orderApplication;
    private readonly IAuctionApplication _auctionApplication;
    private readonly IAccountApplication _accountApplication;
    private readonly IProductApplication _productApplication;

    public AuctionBackgroundService(IAuctionApplication auctionApplication, IAuctionService auctionService, IOrderApplication orderApplication, IProductApplication productApplication, IAccountApplication accountApplication, ICalculateWage calculateWage)
    {
        _calculateWage = calculateWage;
        _auctionService = auctionService;
        _orderApplication = orderApplication;
        _productApplication = productApplication;
        _accountApplication = accountApplication;
        _auctionApplication = auctionApplication;

    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {


        var auctionProducts = await _auctionApplication.GetAuctionsProductId(cancellationToken);
        _auctionService.InitialAuctions(auctionProducts);

        // Update the auction base price and winner id for each product
        foreach (var productId in auctionProducts)
        {
            var bids = _auctionService.GetBids(productId);
            if (bids.Count > 0)
            {
                var maxBid = bids.OrderByDescending(b => b.SuggestionPrice).First();

                await _auctionApplication.SetMaxOffer(productId, maxBid, cancellationToken);
            }
            _auctionService.ClearBids(productId);
        }

        // Check if the auction end date has passed and place an order for the customer
        foreach (var auction in await _auctionApplication.GetEndedAuctionsProducts(cancellationToken))
        {
            if (auction.BasePrice <= 0)
            {
                await _auctionApplication.SuspendAuction(auction, cancellationToken);
                continue;
            }
            var sellerId = await _productApplication.GetProductSeller(auction.ProductId, cancellationToken);
            var cart = new Cart();

            var cartItem = new CartItem(auction.ProductId, sellerId, auction.BasePrice, 1);
            cartItem.CalculateTotalItemPrice();
            var sellerInfo = await _accountApplication.GetFinancialInfo(sellerId, cancellationToken);
            _calculateWage.CalculateWageAsync(cartItem, sellerInfo, cancellationToken);

            cart.Add(cartItem);
            cart.SetPaymentMethod(3);

            await _orderApplication.PlaceAuctionOrder(cart, auction.CustomerId, cancellationToken);
            await _auctionApplication.EndAuctionStatus(auction, cancellationToken);
        }

    }
    public Task ExecuteAsyncPublic(CancellationToken cancellationToken)
    {
        return ExecuteAsync(cancellationToken);
    }
}