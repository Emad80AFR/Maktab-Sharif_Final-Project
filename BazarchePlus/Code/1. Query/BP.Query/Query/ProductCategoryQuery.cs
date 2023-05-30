using BP._Query.Contracts.Product;
using BP._Query.Contracts.ProductCategory;
using DM._Infrastructure.EFCore;
using FrameWork.Application;
using IM._Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SM._Domain.ProductAgg;
using SM._Infrastructure.EFCore;

namespace BP._Query.Query;

public class ProductCategoryQuery:IProductCategoryQuery
{
    private readonly ILogger<ProductCategoryQuery> _logger;
    private readonly ShopContext _context;
    private readonly InventoryContext _inventoryContext;
    private readonly DiscountContext _discountContext;

    public ProductCategoryQuery(ShopContext context, ILogger<ProductCategoryQuery> logger, InventoryContext inventoryContext, DiscountContext discountContext)
    {
        _context = context;
        _logger = logger;
        _discountContext = discountContext;
        _inventoryContext = inventoryContext;
    }

    public async Task<List<ProductCategoryQueryModel>> GetProductCategories(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching product categories...");

            var productCategories = await _context.ProductCategories
                .Select(x => new ProductCategoryQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Product categories retrieved successfully.");

            return productCategories;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching product categories.");
            throw;
        }
    }


    public async Task<List<ProductCategoryQueryModel>> GetProductCategoriesWithProducts(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching product categories with products...");

            var inventory = await _inventoryContext.Inventory
                .Select(x => new { x.ProductId, x.UnitPrice })
                .ToListAsync(cancellationToken);

            var discounts = await _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new { x.DiscountRate, x.ProductId })
                .ToListAsync(cancellationToken);

            var categories = await _context.ProductCategories
                .Include(x => x.Products)
                .ThenInclude(x => x.Category)
                .Select(x => new ProductCategoryQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Products = MapProducts(x.Products)
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            foreach (var category in categories)
            {
                foreach (var product in category.Products)
                {
                    var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                    if (productInventory == null) continue;
                    {
                        var price = productInventory.UnitPrice;
                        product.Price = price.ToMoney();
                        var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                        if (discount == null) continue;
                        var discountRate = discount.DiscountRate;
                        product.DiscountRate = discountRate;
                        product.HasDiscount = discountRate > 0;
                        var discountAmount = Math.Round((price * discountRate) / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }

            if (categories != null && categories.Any())
                _logger.LogInformation("Product categories with products retrieved successfully. Total categories: {CategoryCount}", categories.Count);
            else
                _logger.LogWarning("No product categories found.");

            return categories;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching product categories with products.");
            throw; 
        }
    }

    private static List<ProductQueryModel> MapProducts(IEnumerable<Product> products)
    {
        return products.Select(product => new ProductQueryModel
        {
            Id = product.Id,
            Category = product.Category.Name,
            Name = product.Name,
            Picture = product.Picture,
            PictureAlt = product.PictureAlt,
            PictureTitle = product.PictureTitle,
            Slug = product.Slug
        }).ToList();
    }

    public async Task<ProductCategoryQueryModel> GetProductCategoryWithProductsBy(string slug, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching product category with products by slug: {Slug}", slug);

            var inventory = await _inventoryContext.Inventory
                .Select(x => new { x.ProductId, x.UnitPrice })
                .ToListAsync(cancellationToken);

            var discounts = await _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate })
                .ToListAsync(cancellationToken);

            var category = await _context.ProductCategories
                .Include(a => a.Products)
                .ThenInclude(x => x.Category)
                .Select(x => new ProductCategoryQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    MetaDescription = x.MetaDescription,
                    Keywords = x.Keywords,
                    Slug = x.Slug,
                    Products = MapProducts(x.Products)
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);

            if (category != null)
            {
                foreach (var product in category.Products)
                {
                    var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                    if (productInventory == null) continue;
                    {
                        var price = productInventory.UnitPrice;
                        product.Price = price.ToMoney();
                        var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                        if (discount == null) continue;
                        var discountRate = discount.DiscountRate;
                        product.DiscountRate = discountRate;
                        product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                        product.HasDiscount = discountRate > 0;
                        var discountAmount = Math.Round((price * discountRate) / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }

            if (category != null)
                _logger.LogInformation("Product category with products retrieved successfully. Category: {CategoryName}", category.Name);
            else
                _logger.LogWarning("No product category found for slug: {Slug}", slug);

            return category;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching product category with products for slug: {Slug}", slug);
            throw; 
        }
    }

}