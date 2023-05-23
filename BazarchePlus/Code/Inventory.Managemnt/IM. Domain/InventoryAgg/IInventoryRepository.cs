using FrameWork.Domain;
using IM._Application.Contracts.Inventory.DTO_s;

namespace IM._Domain.InventoryAgg;

public interface IInventoryRepository:IBaseRepository<long,Inventory>
{
    Task<EditInventory?> GetDetails(long id);
    Task<Inventory?> GetBy(long productId);
    Task<List<InventoryViewModel>> Search(InventorySearchModel searchModel);
    Task<List<InventoryOperationViewModel>> GetOperationLog(long inventoryId);

}