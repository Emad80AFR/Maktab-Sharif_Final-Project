namespace AM._Application.Contracts.Auction.DTO_s;

public class AuctionProcessModel
{
    public long ProductId { get;  set; }
    public DateTime EndDate { get;  set; }
    public bool IsActive { get;  set; }
    public long CustomerId { get; set; }
    public double BasePrice { get; set; }
    public long BidsCount { get; set; }
    public string? WinnerUsername { get; set; }
    public int Status { get; set; }
    public long Id { get; set; }
    public string CreationDate { get; set; }
    public string ProductName { get; set; }

}