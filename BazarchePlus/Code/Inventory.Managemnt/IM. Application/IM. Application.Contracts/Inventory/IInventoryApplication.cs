using FrameWork.Application;
using IM._Application.Contracts.Inventory.DTO_s;

namespace IM._Application.Contracts.Inventory;

public interface IInventoryApplication
{
    Task<OperationResult> Create(CreateInventory command, CancellationToken cancellationToken);
    Task<OperationResult> Edit(EditInventory command, CancellationToken cancellationToken);
    Task<OperationResult> Increase(IncreaseInventory command, CancellationToken cancellationToken);
    Task<OperationResult> Reduce(ReduceInventory command, CancellationToken cancellationToken);
    Task<OperationResult> Reduce(List<ReduceInventory> command, CancellationToken cancellationToken);
    Task<EditInventory> GetDetails(long id, CancellationToken cancellationToken);
    Task<List<InventoryViewModel>> Search(InventorySearchModel searchModel, CancellationToken cancellationToken);
    Task<List<InventoryOperationViewModel>> GetOperationLog(long inventoryId, CancellationToken cancellationToken);

}