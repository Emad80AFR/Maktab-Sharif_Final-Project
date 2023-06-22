using SM._Domain.OrderAgg;

namespace SM._Domain.Services;

public interface IShopInventoryAcl
{
    Task<bool> ReduceFromInventory(List<OrderItem> items,CancellationToken cancellationToken);
}