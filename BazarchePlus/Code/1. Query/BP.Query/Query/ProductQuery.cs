using BP._Query.Contracts.Comment;
using BP._Query.Contracts.Product;
using CM._Infrastructure.EFCore;
using DM._Infrastructure.EFCore;
using FrameWork.Application;
using IM._Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SM._Domain.ProductPictureAgg;
using SM._Infrastructure.EFCore;

namespace BP._Query.Query
{
    public class ProductQuery : IProductQuery
    {
        private readonly ILogger<ProductQuery> _logger;
        private readonly ShopContext _context;
        private readonly InventoryContext _inventoryContext;
        private readonly DiscountContext _discountContext;
        private readonly CommentContext _commentContext;

        public ProductQuery(ShopContext context, InventoryContext inventoryContext,
            DiscountContext discountContext, ILogger<ProductQuery> logger, CommentContext commentContext)
        {
            _context = context;
            _discountContext = discountContext;
            _logger = logger;
            _inventoryContext = inventoryContext;
            _commentContext = commentContext;
        }


        public async Task<ProductQueryModel> GetProductDetails(string slug, CancellationToken cancellationToken)
        {

            try
            {
                var inventory = await _inventoryContext.Inventory
                    .Select(x => new { x.ProductId, x.UnitPrice, x.InStock })
                    .ToListAsync(cancellationToken);

                var discounts = await _discountContext.CustomerDiscounts
                    .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                    .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate })
                    .ToListAsync(cancellationToken);

                var product = await _context.Products
                    .Include(x => x.Category)
                    .Include(x => x.ProductPictures)
                    .Select(x => new ProductQueryModel
                    {
                        Id = x.Id,
                        Category = x.Category.Name,
                        Name = x.Name,
                        Picture = x.Picture,
                        PictureAlt = x.PictureAlt,
                        PictureTitle = x.PictureTitle,
                        Slug = x.Slug,
                        CategorySlug = x.Category.Slug,
                        Code = x.Code,
                        Description = x.Description,
                        Keywords = x.Keywords,
                        MetaDescription = x.MetaDescription,
                        ShortDescription = x.ShortDescription,
                        Pictures = MapProductPictures(x.ProductPictures)
                    }).AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);

                if (product == null)
                    return new ProductQueryModel();

                var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                if (productInventory != null)
                {
                    product.IsInStock = productInventory.InStock;
                    var price = productInventory.UnitPrice;
                    product.Price = price.ToMoney();
                    product.DoublePrice = price;
                    var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                    if (discount != null)
                    {
                        var discountRate = discount.DiscountRate;
                        product.DiscountRate = discountRate;
                        product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                        product.HasDiscount = discountRate > 0;
                        var discountAmount = Math.Round((price * discountRate) / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }

                product.Comments = await _commentContext.Comments
                    .Where(x => !x.IsCanceled)
                    .Where(x => x.IsConfirmed)
                    .Where(x => x.Type == CommentType.Product)
                    .Where(x => x.OwnerRecordId == product.Id)
                    .Select(x => new CommentQueryModel
                    {
                        Id = x.Id,
                        Message = x.Message,
                        Name = x.Name,
                        CreationDate = x.CreationDate.ToFarsi()
                    }).OrderByDescending(x => x.Id).ToListAsync(cancellationToken);

                _logger.LogInformation("GetProductDetails method executed successfully.");

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetProductDetails method.");
                throw;
            }
        }
        private static List<ProductPictureQueryModel> MapProductPictures(List<ProductPicture> pictures)
        {
            return pictures.Select(x => new ProductPictureQueryModel
            {
                IsRemoved = x.IsRemoved,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ProductId = x.ProductId
            }).Where(x => !x.IsRemoved).ToList();
        }

        public async Task<List<ProductQueryModel>> GetLatestArrivals(CancellationToken cancellationToken)
        {
            try
            {
                var inventory = await _inventoryContext.Inventory
                    .Select(x => new { x.ProductId, x.UnitPrice })
                    .ToListAsync(cancellationToken);

                var discounts = await _discountContext.CustomerDiscounts
                    .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                    .Select(x => new { x.DiscountRate, x.ProductId })
                    .ToListAsync(cancellationToken);

                var products = await _context.Products.Include(x => x.Category)
                    .Select(product => new ProductQueryModel
                    {
                        Id = product.Id,
                        Category = product.Category.Name,
                        Name = product.Name,
                        Picture = product.Picture,
                        PictureAlt = product.PictureAlt,
                        PictureTitle = product.PictureTitle,
                        Slug = product.Slug
                    })
                    .AsNoTracking()
                    .OrderByDescending(x => x.Id)
                    .Take(6)
                    .ToListAsync(cancellationToken);

                foreach (var product in products)
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

                _logger.LogInformation("GetLatestArrivals method executed successfully.");

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetLatestArrivals method.");
                throw;
            }
        }

        public async Task<List<ProductQueryModel>> Search(string value, CancellationToken cancellationToken)
        {
            try
            {
                var inventory = await _inventoryContext.Inventory
                    .Select(x => new { x.ProductId, x.UnitPrice })
                    .ToListAsync(cancellationToken);

                var discounts = await _discountContext.CustomerDiscounts
                    .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                    .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate })
                    .ToListAsync(cancellationToken);

                var query = _context.Products
                    .Include(x => x.Category)
                    .Select(product => new ProductQueryModel
                    {
                        Id = product.Id,
                        Category = product.Category.Name,
                        CategorySlug = product.Category.Slug,
                        Name = product.Name,
                        Picture = product.Picture,
                        PictureAlt = product.PictureAlt,
                        PictureTitle = product.PictureTitle,
                        ShortDescription = product.ShortDescription,
                        Slug = product.Slug
                    }).AsNoTracking();

                if (!string.IsNullOrWhiteSpace(value))
                    query = query.Where(x => x.Name.Contains(value) || x.ShortDescription.Contains(value));

                var products = await query.OrderByDescending(x => x.Id).ToListAsync(cancellationToken);

                foreach (var product in products)
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

                _logger.LogInformation("Search method executed successfully.");

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in Search method.");
                throw;
            }
        }
    }
}