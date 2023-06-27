namespace SM._Application.Contracts.Order.DTO_s;

public class CartItem
{
    public long Id { get; set; }
    public long SellerId { get; set; }
    public string Name { get; set; }
    public double UnitPrice { get; set; }
    public int WageRate { get; set; }
    public double Wage { get; set; }
    public string Picture { get; set; }
    public int Count { get; set; }
    public double TotalItemPrice { get; set; }
    public double TotalItemWage { get; set; }
    public bool IsInStock { get; set; }
    public int DiscountRate { get; set; }
    public double DiscountAmount { get; set; }
    public double ItemPayAmount { get; set; }

    public CartItem()
    {
        TotalItemPrice = Count * UnitPrice;
    }

    public CartItem(long id, long sellerId, double unitPrice, int count)
    {
        Id = id;
        SellerId = sellerId;
        UnitPrice = unitPrice;
        Count = count;
    }

    public void CalculateTotalItemPrice()
    {
        TotalItemPrice = UnitPrice * Count;
    }
}