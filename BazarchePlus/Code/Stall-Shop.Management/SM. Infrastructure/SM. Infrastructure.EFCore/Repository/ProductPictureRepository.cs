using FrameWork.Application;
using FrameWork.Application.Authentication;
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
    private readonly IAuthHelper _authHelper;

    public ProductPictureRepository(ILogger<ProductPictureRepository> logger, ShopContext context, IAuthHelper authHelper):base(context,logger)
    {
        _logger = logger;
        _context = context;
        _authHelper = authHelper;
    }

    public async Task<EditProductPicture> GetDetails(long id, CancellationToken cancellationToken)
    {
        var productPicture = await _context.ProductPictures
            .Select(x => new EditProductPicture
            {
                Id = x.Id,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ProductId = x.ProductId
            })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

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

    public async Task<ProductPicture> GetWithProductAndCategory(long id, CancellationToken cancellationToken)
    {
        var productPicture = await _context.ProductPictures
            .Include(x => x.Product)
            .ThenInclude(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (productPicture == null)
        {
            _logger.LogWarning("No product picture found with ID: {ProductPictureId}", id);
            return null!;
        }

        _logger.LogInformation("Retrieved product picture successfully with ID: {ProductPictureId}", id);

        return productPicture;
    }

    public async Task<List<ProductPictureViewModel>> Search(ProductPictureSearchModel searchModel, CancellationToken cancellationToken)
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
                IsRemoved = x.IsRemoved,
                ProductSellerId = x.Product.SellerId

            });

        var role = _authHelper.CurrentAccountRole();
        if (role == Roles.Seller)
        {
            query = query.Where(x => x.ProductSellerId == _authHelper.CurrentAccountId());
        }
        if (searchModel.ProductId != 0)
        {
            query = query.Where(x => x.ProductId == searchModel.ProductId);
        }

        var productPictures = await query.OrderByDescending(x => x.Id).ToListAsync(cancellationToken: cancellationToken);

        // Log information 
        _logger.LogInformation("Retrieved product pictures successfully.");

        return productPictures;
    }
}