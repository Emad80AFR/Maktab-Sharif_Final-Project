using FrameWork.Domain;

namespace AM._Domain.AuctionAgg;

public class Auction:BaseClass<long>
{
    public long ProductId { get;private set; }
    public DateTime EndDate { get; private set; }
    public bool IsActive { get; private set; }
    public long CustomerId { get; private set; }
    public double BasePrice { get; private set; }
    public long BidsCount { get; private set; }
    public string WinnerUsername { get; private set; }
    public int Status { get; set; }


    public Auction(long productId, DateTime endDate,double basePrice)
    {
        ProductId = productId;
        EndDate = endDate;
        IsActive = false;
        BasePrice=basePrice;
        Status = (int)AuctionStatus.Waiting;
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

    public void AddBid(long userId,double bidPrice)
    {
        BidsCount += 1;
        BasePrice = bidPrice;
        CustomerId=userId;
    }
}