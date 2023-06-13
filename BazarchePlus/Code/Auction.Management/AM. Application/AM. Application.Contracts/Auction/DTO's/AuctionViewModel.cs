﻿namespace AM._Application.Contracts.Auction.DTO_s;

public class AuctionViewModel
{
    public long Id { get; set; }
    public long IsActive { get; set; }
    public long ProductId { get; set; }
    public string EndDate { get; set; }
    public double BasePrice { get; set; }
    public string ProductName { get; set; }
    public int Status { get; set; }

}