namespace AM._Application.Contracts.Auction.DTO_s;

public class AuctionViewModel
{
    public long Id { get; set; }
    public long SellerId { get; set; }
    public long CustomerId { get; set; }
    public bool IsActive { get; set; }
    public long ProductId { get; set; }
    public string EndDate { get; set; }
    public string CreationDate { get; set; }
    public double BasePrice { get; set; }
    public string ProductName { get; set; }
    public string CustomerName { get; set; }
    public int Status { get; set; }
}