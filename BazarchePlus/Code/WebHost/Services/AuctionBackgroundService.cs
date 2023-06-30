using AM._Application.Contracts.Auction;
using SM._Application.Contracts.Order;
using SM._Application.Contracts.Order.DTO_s;
using SM._Application.Contracts.Product;

namespace WebHost.Services;

public class AuctionBackgroundService:BackgroundService
{
    private readonly IAuctionService _auctionService;
    private readonly IOrderApplication _orderApplication ;
    private readonly IAuctionApplication _auctionApplication;
    private readonly IProductApplication _productApplication ;
    public AuctionBackgroundService(IAuctionApplication auctionApplication, IAuctionService auctionService, IOrderApplication orderApplication, IProductApplication productApplication)
    {
        _auctionService = auctionService;
        _orderApplication = orderApplication;
        _productApplication = productApplication;
        _auctionApplication = auctionApplication;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        

            var auctionProducts = await _auctionApplication.GetAuctionsProductId(cancellationToken);
            _auctionService.InitialAuctions(auctionProducts);

            // Update the auction base price and winner id for each product
            foreach (var productId in auctionProducts)
            {
                var bids=_auctionService.GetBids(productId);
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
                var sellerId= await _productApplication.GetProductSeller(auction.ProductId, cancellationToken);
                var cart = new Cart();
                var cartItem=new CartItem(auction.ProductId,sellerId,auction.BasePrice,1);
                cart.Add(cartItem);
                cart.SetPaymentMethod(3);

                await _orderApplication.PlaceAuctionOrder(cart, auction.CustomerId, cancellationToken);
                await _auctionApplication.EndAuctionStatus(auction, cancellationToken);
            }
        
    }
    public  Task ExecuteAsyncPublic(CancellationToken cancellationToken)
    {
        return  ExecuteAsync(cancellationToken);
    }
}