using AM._Infrastructure.EFCore;
using FrameWork.Application;
using FrameWork.Application.Authentication;
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
    private readonly AccountContext _accountContext; 
    private readonly IAuthHelper _authHelper;


    public InventoryRepository(ILogger<InventoryRepository> logger, InventoryContext context, ShopContext shopContext, AccountContext accountContext, IAuthHelper authHelper):base(context,logger)
    {
        _logger = logger;
        _inventoryContext = context;
        _shopContext = shopContext;
        _accountContext = accountContext;
        _authHelper = authHelper;
    }

    public async Task<EditInventory> GetDetails(long id,CancellationToken cancellationToken)
    {
        var inventory = await _inventoryContext.Inventory
            .Select(x => new EditInventory
            {
                Id = x.Id,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice
            })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (inventory == null)
        {
            
            _logger.LogWarning("Inventory not found for {Id}:",id);
        }

        return inventory!;
    }

    public async Task<Inventory> GetBy(long productId, CancellationToken cancellationToken)
    {
        var inventory = await _inventoryContext.Inventory.FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken: cancellationToken);

        if (inventory == null)
        {
            
            _logger.LogWarning($"Inventory not found for productId: {productId}");
        }

        return inventory!; ;
    }

    public async Task<List<InventoryViewModel>> Search(InventorySearchModel searchModel, CancellationToken cancellationToken)
    {
        
        

        var query = _inventoryContext.Inventory.Select(x => new InventoryViewModel
        {
            Id = x.Id,
            UnitPrice = x.UnitPrice,
            InStock = x.InStock,
            ProductId = x.ProductId,
            CurrentCount = x.CalculateCurrentCount(),
            CreationDate = x.CreationDate.ToFarsi(),
            SellerId = x.SellerId
        });
        var productQuery = _shopContext.Products.Select(x => new { x.Id, x.Name, x.SellerId });
        var role = _authHelper.CurrentAccountRole();
        if (role == Roles.Seller)
        {
            query = query.Where(x => x.SellerId == _authHelper.CurrentAccountId());
            productQuery = productQuery.Where(x => x.SellerId == _authHelper.CurrentAccountId());
        }
        var products = await productQuery.ToListAsync(cancellationToken: cancellationToken);
        if (searchModel.ProductId > 0)
            query = query.Where(x => x.ProductId == searchModel.ProductId);

        if (searchModel.InStock)
            query = query.Where(x => !x.InStock);

        var inventory = await query.OrderByDescending(x => x.Id).ToListAsync(cancellationToken: cancellationToken);

        inventory.ForEach(item =>
            item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name!);

        return inventory;
    }

    public async Task<List<InventoryOperationViewModel>> GetOperationLog(long inventoryId, CancellationToken cancellationToken)
    {
        var accounts = await _accountContext.Accounts.Select(x => new { x.Id, x.Fullname }).ToListAsync(cancellationToken);
        var inventory = await _inventoryContext.Inventory.FirstOrDefaultAsync(x => x.Id == inventoryId, cancellationToken);

        if (inventory == null)
        {
            
            _logger.LogWarning("Inventory not found for ID: {InventoryId}", inventoryId);
            return new List<InventoryOperationViewModel>();
        }

        var operations = inventory.Operations.Select(x => new InventoryOperationViewModel
        {
            Id = x.Id,
            Count = x.Count,
            CurrentCount = x.CurrentCount,
            Description = x.Description,
            Operation = x.Operation,
            OperationDate = x.OperationDate.ToFarsi(),
            OperatorId = x.OperatorId,
            OrderId = x.OrderId
        }).OrderByDescending(x => x.Id).ToList();

        foreach (var operation in operations)
        {
            operation.Operator = accounts.FirstOrDefault(x => x.Id == operation.OperatorId)?.Fullname!;
        }

        
        _logger.LogInformation("Retrieved operation log for inventory ID: {InventoryId}", inventoryId);

        return operations;
    }
}