using System.ComponentModel.DataAnnotations;
using FrameWork.Application;
using SM._Application.Contracts.Product.DTO_s;

namespace IM._Application.Contracts.Inventory.DTO_s;

public class CreateInventory
{
    [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
    public long ProductId { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
    public double UnitPrice { get; set; }

    public List<ProductViewModel> Products { get; set; }

}