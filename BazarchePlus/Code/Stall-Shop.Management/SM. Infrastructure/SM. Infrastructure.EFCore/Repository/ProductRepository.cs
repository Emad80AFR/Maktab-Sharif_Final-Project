using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SM._Application.Contracts.Product.DTO_s;
using SM._Domain.ProductAgg;

namespace SM._Infrastructure.EFCore.Repository;

public class ProductRepository:BaseRepository<long,Product>,IProductRepository
{
    private readonly ShopContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(ILogger<ProductRepository> logger, ShopContext context):base(context,logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<EditProduct> GetDetails(long id, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Where(x => x.Id == id)
            .Select(x => new EditProduct
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Slug = x.Slug,
                CategoryId = x.CategoryId,
                Description = x.Description,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ShortDescription = x.ShortDescription,
            })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (product == null)
        {
            // Log warning 
            _logger.LogWarning("No product found with ID: {Id}",id);
            return null!;
        }

        // Log information 
        _logger.LogInformation("Retrieved product with ID: {Id} successfully.",id);

        return product;
    }

    public async Task<Product> GetProductWithCategory(long id, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (product == null)
        {
            // Log warning 
            _logger.LogWarning("No product found with ID: {Id}", id);
            return null;
        }

        // Log information 
        _logger.LogInformation("Retrieved product with ID: {Id} successfully.", id);

        return product;
    }

    public async Task<List<ProductViewModel>> GetProducts(CancellationToken cancellationToken)
    {
        var products = await _context.Products
            .Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync(cancellationToken: cancellationToken);

        // Log information 
        _logger.LogInformation("Retrieved product list successfully.");

        return products;
    }

    public async Task<List<ProductViewModel>> Search(ProductSearchModel searchModel, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .Include(x => x.Category)
            .Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.Category.Name,
                CategoryId = x.CategoryId,
                Code = x.Code,
                Picture = x.Picture,
                CreationDate = x.CreationDate.ToFarsi()
            });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
            query = query.Where(x => x.Name.Contains(searchModel.Name));

        if (!string.IsNullOrWhiteSpace(searchModel.Code))
            query = query.Where(x => x.Code.Contains(searchModel.Code));

        if (searchModel.CategoryId != 0)
            query = query.Where(x => x.CategoryId == searchModel.CategoryId);

        var productList = await query
            .OrderByDescending(x => x.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        // Log information 
        _logger.LogInformation("Retrieved product list successfully.");

        return productList;
    }
}