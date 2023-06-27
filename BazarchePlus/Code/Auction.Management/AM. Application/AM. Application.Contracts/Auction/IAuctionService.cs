using AM._Application.Contracts.Auction.DTO_s;

namespace AM._Application.Contracts.Auction;

public interface IAuctionService
{
    void InitialAuctions(List<long> products);
    List<Bid> GetBids(long id);
    void AddBid(Bid bid,long productId);
    void ClearBids(long id);

}