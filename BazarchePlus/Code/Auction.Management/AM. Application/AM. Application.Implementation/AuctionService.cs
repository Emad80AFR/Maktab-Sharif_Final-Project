using AM._Application.Contracts.Auction;
using AM._Application.Contracts.Auction.DTO_s;
using AM._Domain.AuctionAgg;

namespace AM._Application.Implementation;

public class AuctionService:IAuctionService
{
    public Dictionary<long, List<Bid>> ProductBids = new();
    public void InitialAuctions(List<long> products)
    {
        products.ForEach(x=>ProductBids[x]=new List<Bid>());
    }

    public List<Bid> GetBids(long id)
    {
        return ProductBids[id];
    }

    public void AddBid(Bid bid,long productId)
    {
        if (!ProductBids.ContainsKey(productId))
        {
            ProductBids[productId] = new List<Bid>();
        }

        ProductBids[productId].Add(bid);
    }

    public void ClearBids(long id)
    {
        ProductBids[id].Clear();
    }

    
}