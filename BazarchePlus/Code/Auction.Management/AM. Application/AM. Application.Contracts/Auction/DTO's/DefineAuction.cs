using SM._Application.Contracts.Product.DTO_s;

namespace AM._Application.Contracts.Auction.DTO_s;

public class DefineAuction
{
    public long ProductId { get; set; }
    public string EndDate { get; set; }
    public double BasePrice { get; set; }
    public List<ProductViewModel> Products { get; set; }
}