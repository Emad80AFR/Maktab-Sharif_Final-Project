using FrameWork.Application;
using IM._Application.Contracts.Inventory;
using IM._Application.Contracts.Inventory.DTO_s;
using IM._Domain.InventoryAgg;
using Microsoft.Extensions.Logging;

namespace IM._Application.Implementation
{
    public class InventoryApplication:IInventoryApplication
    {
        private readonly IAuthHelper _authHelper;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ILogger<InventoryApplication> _logger;

        public InventoryApplication(IAuthHelper authHelper, IInventoryRepository inventoryRepository, ILogger<InventoryApplication> logger)
        {
            _authHelper = authHelper;
            _inventoryRepository = inventoryRepository;
            _logger = logger;
        }

        public async Task<OperationResult> Create(CreateInventory command)
        {
            var operation = new OperationResult();
            if (await _inventoryRepository.Exist(x => x.ProductId == command.ProductId))
            {
                _logger.LogWarning("Duplicated record found when creating inventory for ProductId: {ProductId}", command.ProductId);
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var inventory = new Inventory(command.ProductId, command.UnitPrice);
            _inventoryRepository.Create(inventory);
            try
            {
                await _inventoryRepository.SaveChanges();
                _logger.LogInformation("Inventory created successfully for ProductId: {ProductId}", command.ProductId);
                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating inventory for ProductId: {ProductId}", command.ProductId);
                return operation.Failed(ApplicationMessages.ErrorOccurred);
            }
        }

        public async Task<OperationResult> Edit(EditInventory command)
        {
            var operation = new OperationResult();
            var inventory = await _inventoryRepository.Get(command.Id);
            if (inventory == null)
            {
                _logger.LogWarning("Inventory record not found for Id: {InventoryId}", command.Id);
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            if (await _inventoryRepository.Exist(x => x.ProductId == command.ProductId && x.Id != command.Id))
            {
                _logger.LogWarning("Duplicated record found when updating inventory for ProductId: {ProductId}", command.ProductId);
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            inventory.Edit(command.ProductId, command.UnitPrice);
            try
            {
                await _inventoryRepository.SaveChanges();
                _logger.LogInformation("Inventory updated successfully for Id: {InventoryId}", command.Id);
                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating inventory for Id: {InventoryId}", command.Id);
                return operation.Failed(ApplicationMessages.ErrorOccurred);
            }
        }

        public async Task<OperationResult> Increase(IncreaseInventory command)
        {
            var operation = new OperationResult();
            var inventory = await _inventoryRepository.Get(command.InventoryId);
            if (inventory == null)
            {
                _logger.LogWarning("Inventory not found for InventoryId: {InventoryId}", command.InventoryId);
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            const long operatorId = 1;
            inventory.Increase(command.Count, operatorId, command.Description);
            await _inventoryRepository.SaveChanges();

            _logger.LogInformation("Inventory count increased successfully for InventoryId: {InventoryId}", command.InventoryId);
            return operation.Succeeded();
        }

        public async Task<OperationResult> Reduce(ReduceInventory command)
        {
            var operation = new OperationResult();
            var inventory = await _inventoryRepository.Get(command.InventoryId);
            if (inventory == null)
            {
                _logger.LogWarning("Inventory not found for InventoryId: {InventoryId}", command.InventoryId);
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            var operatorId = _authHelper.CurrentAccountId();
            inventory.Reduce(command.Count, operatorId, command.Description, 0);
            await _inventoryRepository.SaveChanges();

            _logger.LogInformation("Inventory count reduced successfully for InventoryId: {InventoryId}", command.InventoryId);
            return operation.Succeeded();
        }

        public async Task<OperationResult> Reduce(List<ReduceInventory> command)
        {
            var operation = new OperationResult();
            var operatorId = _authHelper.CurrentAccountId();
            foreach (var item in command)
            {
                var inventory = await _inventoryRepository.GetBy(item.ProductId);
                if (inventory == null)
                {
                    _logger.LogWarning("Inventory not found for ProductId: {ProductId}", item.ProductId);
                    return operation.Failed(ApplicationMessages.RecordNotFound);
                }

                inventory.Reduce(item.Count, operatorId, item.Description, item.OrderId);
            }

            await _inventoryRepository.SaveChanges();

            _logger.LogInformation("Inventory count reduced successfully for multiple products");
            return operation.Succeeded();
        }

        public async Task<EditInventory> GetDetails(long id)
        {
            var inventoryDetails = await _inventoryRepository.GetDetails(id);
            if (inventoryDetails == null)
            {
                _logger.LogWarning("Inventory details not found for Id: {InventoryId}", id);
                return null!;
            }

            return inventoryDetails;
        }

        public async Task<List<InventoryViewModel>> Search(InventorySearchModel searchModel)
        {
            var searchResult=await _inventoryRepository.Search(searchModel);

            _logger.LogInformation("Inventory search completed successfully");

            return searchResult;
        }

        public async Task<List<InventoryOperationViewModel>> GetOperationLog(long inventoryId)
        {
            var operationLog = await _inventoryRepository.GetOperationLog(inventoryId);
            if (operationLog == null)
            {
                _logger.LogWarning("Operation log not found for InventoryId: {InventoryId}", inventoryId);
                return null!;
            }

            return operationLog;
        }
    }
}