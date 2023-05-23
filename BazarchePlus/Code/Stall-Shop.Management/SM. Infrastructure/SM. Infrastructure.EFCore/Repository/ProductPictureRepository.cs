using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SM._Application.Contracts.ProductPicture.DTO_s;
using SM._Domain.ProductPictureAgg;

namespace SM._Infrastructure.EFCore.Repository;

public class ProductPictureRepository:BaseRepository<long,ProductPicture>,IProductPictureRepository
{
    private readonly ILogger<ProductPictureRepository> _logger;
    private readonly ShopContext _context;

    public ProductPictureRepository(ILogger<ProductPictureRepository> logger, ShopContext context):base(context,logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<EditProductPicture> GetDetails(long id)
    {
        var productPicture = await _context.ProductPictures
            .Select(x => new EditProductPicture
            {
                Id = x.Id,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ProductId = x.ProductId
            })
            .FirstOrDefaultAsync(x => x.Id == id);

        if (productPicture == null)
        {
            // Log warning 
            _logger.LogWarning("No product picture found with ID: {ProductPictureId}", id);
            return null!; 
        }

        // Log information 
        _logger.LogInformation("Retrieved product picture successfully with ID: {ProductPictureId}", id);

        return productPicture;
    }

    public async Task<ProductPicture> GetWithProductAndCategory(long id)
    {
        var productPicture = await _context.ProductPictures
            .Include(x => x.Product)
            .ThenInclude(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (productPicture == null)
        {
            _logger.LogWarning("No product picture found with ID: {ProductPictureId}", id);
            return null!;
        }

        _logger.LogInformation("Retrieved product picture successfully with ID: {ProductPictureId}", id);

        return productPicture;
    }

    public async Task<List<ProductPictureViewModel>> Search(ProductPictureSearchModel searchModel)
    {
        var query = _context.ProductPictures
            .Include(x => x.Product)
            .Select(x => new ProductPictureViewModel
            {
                Id = x.Id,
                Product = x.Product.Name,
                CreationDate = x.CreationDate.ToFarsi(),
                Picture = x.Picture,
                ProductId = x.ProductId,
                IsRemoved = x.IsRemoved
            });

        if (searchModel.ProductId != 0)
        {
            query = query.Where(x => x.ProductId == searchModel.ProductId);
        }

        var productPictures = await query.OrderByDescending(x => x.Id).ToListAsync();

        // Log information if needed
        _logger.LogInformation("Retrieved product pictures successfully.");

        return productPictures;
    }
}