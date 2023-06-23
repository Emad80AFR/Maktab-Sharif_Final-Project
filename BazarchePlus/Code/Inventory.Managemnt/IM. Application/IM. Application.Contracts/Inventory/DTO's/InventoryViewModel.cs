namespace IM._Application.Contracts.Inventory.DTO_s;

public class InventoryViewModel
{
    public long Id { get; set; }
    public string Product { get; set; }
    public long ProductId { get; set; }
    public long SellerId { get; set; }
    public double UnitPrice { get; set; }
    public bool InStock { get; set; }
    public long CurrentCount { get; set; }
    public string CreationDate { get; set; }

}