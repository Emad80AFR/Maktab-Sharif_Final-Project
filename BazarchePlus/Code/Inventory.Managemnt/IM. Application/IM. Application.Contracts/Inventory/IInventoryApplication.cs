using FrameWork.Application;
using IM._Application.Contracts.Inventory.DTO_s;

namespace IM._Application.Contracts.Inventory;

public interface IInventoryApplication
{
    Task<OperationResult> Create(CreateInventory command);
    Task<OperationResult> Edit(EditInventory command);
    Task<OperationResult> Increase(IncreaseInventory command);
    Task<OperationResult> Reduce(ReduceInventory command);
    Task<OperationResult> Reduce(List<ReduceInventory> command);
    Task<EditInventory> GetDetails(long id);
    Task<List<InventoryViewModel>> Search(InventorySearchModel searchModel);
    Task<List<InventoryOperationViewModel>> GetOperationLog(long inventoryId);

}