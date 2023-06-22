using IM._Application.Contracts.Inventory;
using IM._Application.Contracts.Inventory.DTO_s;
using SM._Domain.OrderAgg;
using SM._Domain.Services;

namespace SM._Infrastructure.InventoryAcl
{
    public class ShopInventoryAcl:IShopInventoryAcl
    {
        private readonly IInventoryApplication _inventoryApplication;

        public ShopInventoryAcl(IInventoryApplication inventoryApplication)
        {
            _inventoryApplication = inventoryApplication;
        }

        public async Task<bool> ReduceFromInventory(List<OrderItem> items,CancellationToken cancellationToken)
        {
            var command = items.Select(orderItem =>
                    new ReduceInventory(orderItem.ProductId, orderItem.Count, "خرید مشتری", orderItem.OrderId))
                .ToList();

            return (await _inventoryApplication.Reduce(command,cancellationToken)).IsSucceeded;

        }
    }
}