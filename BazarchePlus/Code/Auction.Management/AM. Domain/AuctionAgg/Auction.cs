using FrameWork.Domain;

namespace AM._Domain.AuctionAgg;

public class Auction:BaseClass<long>
{
    public long ProductId { get;private set; }
    public long SellerId { get; set; }
    public DateTime EndDate { get; private set; }
    public bool IsActive { get; private set; }
    public long CustomerId { get; private set; }
    public double BasePrice { get; private set; }
    public long BidsCount { get; private set; }
    public string? WinnerUsername { get; private set; }
    public int Status { get; set; }


    public Auction(long productId, DateTime endDate,double basePrice, long sellerId)
    {
        ProductId = productId;
        EndDate = endDate;
        IsActive = true;
        BasePrice = basePrice;
        Status = (int)AuctionStatus.Waiting;
        SellerId = sellerId;
    }

    public void Edit(long productId, DateTime endDate, double basePrice)
    {
        ProductId = productId;
        EndDate = endDate;
        BasePrice = basePrice;
    }

    public void Activate()
    {
        IsActive=true;
    }
    public void DeActive()
    {
        IsActive = false;
    }

    public void AddBid(double maxBid, long customerId)
    {
        BidsCount += 1;
        if (!(BasePrice < maxBid)) return;
        BasePrice = maxBid;
        CustomerId = customerId;

    }

    public void EndAuction(string winnerUsername)
    {
        Status = (int)AuctionStatus.Finished;
        IsActive=false;
        WinnerUsername=winnerUsername;
    }
}