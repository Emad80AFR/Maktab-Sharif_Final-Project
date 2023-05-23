using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SM._Application.Contracts.ProductCategory.DTO_s;
using SM._Domain.ProductCategoryAgg;

namespace SM._Infrastructure.EFCore.Repository;

public class ProductCategoryRepository:BaseRepository<long,ProductCategory>,IProductCategoryRepository
{
    private readonly ShopContext _context;
    private readonly ILogger<BaseRepository<long, ProductCategory>> _logger;

    public ProductCategoryRepository(ShopContext context, ILogger<BaseRepository<long, ProductCategory>> logger) : base(context, logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ProductCategoryViewModel>> GetProductCategories()
    {

        try
        {
            var productCategories = await _context.ProductCategories
                .Select(x => new ProductCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            // Log information
            _logger.LogInformation("Retrieved product categories successfully.");

            return productCategories;
        }
        catch (Exception ex)
        {
            // Log error
            _logger.LogError(ex, "Error occurred while retrieving product categories.");

            // Log warning
            _logger.LogWarning("Failed to retrieve product categories.");

            throw; 
        }
    }

    public async Task<EditProductCategory?> GetDetails(long id)
    {
        
            try
            {
                var productCategory = await _context.ProductCategories
                    .Where(x => x.Id == id)
                    .Select(x => new EditProductCategory()
                    {
                        Id = x.Id,
                        Description = x.Description,
                        Name = x.Name,
                        Keywords = x.Keywords,
                        MetaDescription = x.MetaDescription,
                        //Picture = x.Picture,
                        PictureAlt = x.PictureAlt,
                        PictureTitle = x.PictureTitle,
                        Slug = x.Slug
                    })
                    .FirstOrDefaultAsync();

                if (productCategory != null)
                {
                    _logger.LogInformation("The operation was successful.");
                }
                else
                {
                    _logger.LogWarning("No product category found with {ID}:",id);
                }

                return productCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving product category with {ID}:",id);
                throw; 
            }
        
    }

    public async Task<string> GetSlugById(long id)
    {
        try
        {
            var productCategorySlug = await _context.ProductCategories
                .Where(x => x.Id == id)
                .Select(x => new { x.Id, x.Slug })
                .FirstOrDefaultAsync();

            if (productCategorySlug != null)
            {
                // Log information if needed
                _logger.LogInformation("Retrieved slug for product category with {ID}: ", id);
                return productCategorySlug.Slug;
            }
            else
            {
                // Log warning if needed
                _logger.LogWarning("No product category found with {ID}:", id);
                return null!;
            }
        }
        catch (Exception ex)
        {
            // Log error
            _logger.LogError(ex, "Error occurred while retrieving slug for product category with {ID}: ", id);
            throw; 
        }
    }

    public async Task<List<ProductCategoryViewModel>> Search(ProductCategorySearchModel searchModel)
    {
        try
        {
            var query = _context.ProductCategories
                .Select(x => new ProductCategoryViewModel
                {
                    Id = x.Id,
                    Picture = x.Picture,
                    Name = x.Name,
                    CreationDate = x.CreationDate.ToFarsi()
                });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
            {
                query = query.Where(x => x.Name.Contains(searchModel.Name));
            }

            var filteredProductCategories = await query
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            // Log information
            _logger.LogInformation("Retrieved filtered product categories successfully.");

            return filteredProductCategories;
        }
        catch (Exception ex)
        {
            // Log error
            _logger.LogError(ex, "Error occurred while retrieving filtered product categories.");

            // Log warning
            _logger.LogWarning("Failed to retrieve filtered product categories.");

            throw; 
        }
    }
}