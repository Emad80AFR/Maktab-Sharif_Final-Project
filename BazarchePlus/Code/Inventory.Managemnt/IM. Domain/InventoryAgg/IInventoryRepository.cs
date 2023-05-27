using FrameWork.Domain;
using IM._Application.Contracts.Inventory.DTO_s;

namespace IM._Domain.InventoryAgg;

public interface IInventoryRepository:IBaseRepository<long,Inventory>
{
    Task<EditInventory?> GetDetails(long id, CancellationToken cancellationToken);
    Task<Inventory?> GetBy(long productId, CancellationToken cancellationToken);
    Task<List<InventoryViewModel>> Search(InventorySearchModel searchModel, CancellationToken cancellationToken);
    Task<List<InventoryOperationViewModel>> GetOperationLog(long inventoryId, CancellationToken cancellationToken);

}