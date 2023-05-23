using FrameWork.Application;
using FrameWork.Infrastructure;
using IM._Application.Contracts.Inventory.DTO_s;
using IM._Domain.InventoryAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SM._Infrastructure.EFCore;

namespace IM._Infrastructure.EFCore.Repository;

public class InventoryRepository:BaseRepository<long,Inventory>,IInventoryRepository
{
    private readonly ILogger<InventoryRepository> _logger;
    private readonly InventoryContext _inventoryContext;
    private readonly ShopContext _shopContext;


    public InventoryRepository(ILogger<InventoryRepository> logger, InventoryContext context, ShopContext shopContext):base(context,logger)
    {
        _logger = logger;
        _inventoryContext = context;
        _shopContext = shopContext;
    }

    public async Task<EditInventory?> GetDetails(long id)
    {
        var inventory = await _inventoryContext.Inventory
            .Select(x => new EditInventory
            {
                Id = x.Id,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice
            })
            .FirstOrDefaultAsync(x => x.Id == id);

        if (inventory == null)
        {
            // Log a warning if inventory is not found for the given id
            _logger.LogWarning($"Inventory not found for id: {id}");
        }

        return inventory;
    }

    public async Task<Inventory?> GetBy(long productId)
    {
        var inventory = await _inventoryContext.Inventory.FirstOrDefaultAsync(x => x.ProductId == productId);

        if (inventory == null)
        {
            // Log a warning if inventory is not found for the given productId
            _logger.LogWarning($"Inventory not found for productId: {productId}");
        }

        return inventory; ;
    }

    public async Task<List<InventoryViewModel>> Search(InventorySearchModel searchModel)
    {
        var products = await _shopContext.Products.Select(x => new { x.Id, x.Name }).ToListAsync();

        var query = _inventoryContext.Inventory.Select(x => new InventoryViewModel
        {
            Id = x.Id,
            UnitPrice = x.UnitPrice,
            InStock = x.InStock,
            ProductId = x.ProductId,
            CurrentCount = x.CalculateCurrentCount(),
            CreationDate = x.CreationDate.ToFarsi()
        });

        if (searchModel.ProductId > 0)
            query = query.Where(x => x.ProductId == searchModel.ProductId);

        if (searchModel.InStock)
            query = query.Where(x => !x.InStock);

        var inventory = await query.OrderByDescending(x => x.Id).ToListAsync();

        inventory.ForEach(item =>
            item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name!);

        return inventory;
    }

    public Task<List<InventoryOperationViewModel>> GetOperationLog(long inventoryId)
    {
        throw new NotImplementedException();
    }
}