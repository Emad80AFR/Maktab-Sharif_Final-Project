using FrameWork.Application;
using Microsoft.Extensions.Logging;
using SM._Application.Contracts.ProductPicture;
using SM._Application.Contracts.ProductPicture.DTO_s;
using SM._Domain.ProductAgg;
using SM._Domain.ProductPictureAgg;

namespace SM._Application.Implementation;

public class ProductPictureApplication:IProductPictureApplication
{
    private readonly ILogger<ProductPictureApplication> _logger;
    private readonly IProductPictureRepository _productPictureRepository;
    private readonly IProductRepository _productRepository;
    private readonly IFileUploader _fileUploader;


    public ProductPictureApplication(ILogger<ProductPictureApplication> logger, IProductPictureRepository productPictureRepository, IFileUploader fileUploader, IProductRepository productRepository)
    {
        _logger = logger;
        _productPictureRepository = productPictureRepository;
        _fileUploader = fileUploader;
        _productRepository = productRepository;
    }

    public async Task<OperationResult> Create(CreateProductPicture command)
    {
        var operation = new OperationResult();

        //if (_productPictureRepository.Exists(x => x.Picture == command.Picture && x.ProductId == command.ProductId))
        //{
        //    // Log warning 
        //    _logger.LogWarning("Duplicated product picture record: Picture={Picture}, ProductId={ProductId}", command.Picture, command.ProductId);
        //    return operation.Failed(ApplicationMessages.DuplicatedRecord);
        //}

        var product = await _productRepository.GetProductWithCategory(command.ProductId);

        if (product == null)
        {
            // Log warning 
            _logger.LogWarning("Product not found with ID: {ProductId}", command.ProductId);
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        var path = $"{product.Category.Slug}//{product.Slug}";
        var picturePath = await _fileUploader.Upload(command.Picture, path);

        var productPicture = new ProductPicture(command.ProductId, picturePath, command.PictureAlt, command.PictureTitle);
        await _productPictureRepository.Create(productPicture);
        await _productPictureRepository.SaveChanges();

        // Log information 
        _logger.LogInformation("Product picture created successfully: ProductId={ProductId}, PicturePath={PicturePath}", command.ProductId, picturePath);

        return operation.Succeeded();
    }

    public async Task<OperationResult> Edit(EditProductPicture command)
    {
        var operation = new OperationResult();
        var productPicture = await _productPictureRepository.GetWithProductAndCategory(command.Id);

        if (productPicture == null)
        {
            // Log warning 
            _logger.LogWarning("Product picture not found with ID: {ProductPictureId}", command.Id);
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        var path = $"{productPicture.Product.Category.Slug}//{productPicture.Product.Slug}";
        var picturePath = await _fileUploader.Upload(command.Picture, path);

        productPicture.Edit(command.ProductId, picturePath, command.PictureAlt, command.PictureTitle);
        await _productPictureRepository.SaveChanges();

        // Log information 
        _logger.LogInformation("Product picture updated successfully: ProductPictureId={ProductPictureId}, PicturePath={PicturePath}", command.Id, picturePath);

        return operation.Succeeded();
    }

    public async Task<OperationResult> Remove(long id)
    {
        var operation = new OperationResult();
        var productPicture = await _productPictureRepository.Get(id);

        if (productPicture == null)
        {
            // Log warning 
            _logger.LogWarning("Product picture not found with ID: {ProductPictureId}", id);
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        productPicture.Remove();
        await _productPictureRepository.SaveChanges();

        // Log information 
        _logger.LogInformation("Product picture removed successfully: ProductPictureId={ProductPictureId}", id);

        return operation.Succeeded();
    }

    public async Task<OperationResult> Restore(long id)
    {
        var operation = new OperationResult();
        var productPicture = await _productPictureRepository.Get(id);

        if (productPicture == null)
        {
            // Log warning 
            _logger.LogWarning("Product picture not found with ID: {ProductPictureId}", id);
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        productPicture.Restore();
        await _productPictureRepository.SaveChanges();

        // Log information 
        _logger.LogInformation("Product picture restored successfully: ProductPictureId={ProductPictureId}", id);

        return operation.Succeeded();
    }

    public async Task<EditProductPicture> GetDetails(long id)
    {
        return await _productPictureRepository.GetDetails(id);
    }

    public async Task<List<ProductPictureViewModel>> Search(ProductPictureSearchModel searchModel)
    {
        return await _productPictureRepository.Search(searchModel);
    }
}