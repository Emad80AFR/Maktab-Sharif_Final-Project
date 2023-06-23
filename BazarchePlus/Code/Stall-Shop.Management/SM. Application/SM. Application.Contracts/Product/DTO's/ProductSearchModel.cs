namespace SM._Application.Contracts.Product.DTO_s;

public class ProductSearchModel
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public long CategoryId { get; set; }
    public bool IsActive { get; set; }

}