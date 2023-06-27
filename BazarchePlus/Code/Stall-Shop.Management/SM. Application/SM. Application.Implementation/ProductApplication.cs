using FrameWork.Application;
using FrameWork.Application.FileUpload;
using FrameWork.Application.Messages;
using Microsoft.Extensions.Logging;
using SM._Application.Contracts.Product;
using SM._Application.Contracts.Product.DTO_s;
using SM._Domain.ProductAgg;
using SM._Domain.ProductCategoryAgg;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SM._Application.Implementation;

public class ProductApplication:IProductApplication
{
    private readonly IProductRepository _productRepository;
    private readonly IFileUploader _fileUploader;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly ILogger<ProductApplication> _logger;
    public ProductApplication(IProductRepository productRepository, ILogger<ProductApplication> logger, IFileUploader fileUploader, IProductCategoryRepository productCategoryRepository)
    {
        _productRepository = productRepository;
        _logger = logger;
        _fileUploader = fileUploader;
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task<OperationResult> Create(CreateProduct command, CancellationToken cancellationToken)
    {
        var operation = new OperationResult();

        if (await _productRepository.Exist(x => x.Name == command.Name, cancellationToken))
        {
            // Log error 
            _logger.LogError("Product creation failed due to duplicated record: {ProductName}", command.Name);
            return operation.Failed(ApplicationMessages.DuplicatedRecord);
        }

        var slug = command.Slug.Slugify();
        var categorySlug = await _productCategoryRepository.GetSlugById(command.CategoryId, cancellationToken);
        var path = $"{categorySlug}//{slug}";

        try
        {
            var picturePath = await _fileUploader.Upload(command.Picture, path,  cancellationToken);
            var product = new Product(command.Name, command.Code,
                command.ShortDescription, command.Description, picturePath,
                command.PictureAlt, command.PictureTitle, command.CategoryId, slug,
                command.Keywords, command.MetaDescription,command.SellerId);

            await _productRepository.Create(product, cancellationToken);
            await _productRepository.SaveChanges(cancellationToken);

            // Log information 
            _logger.LogInformation("Product created successfully: {ProductName}", command.Name);

            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            // Log error 
            _logger.LogError(ex, "An error occurred during product creation: {ErrorMessage}", ex.Message);
            return operation.Failed(ApplicationMessages.ErrorOccurred);
        }
    }

    public async Task<OperationResult> Edit(EditProduct command,CancellationToken cancellationToken)
    {
        var operation = new OperationResult();
        var product = await _productRepository.GetProductWithCategory(command.Id, cancellationToken);

        if (product == null)
        {
            // Log warning 
            _logger.LogWarning("No product found with ID: {ProductId}", command.Id);
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        if (await _productRepository.Exist(x => x.Name == command.Name && x.Id != command.Id, cancellationToken))
        {
            // Log warning 
            _logger.LogWarning("Product creation failed due to duplicated record: {ProductName}", command.Name);
            return operation.Failed(ApplicationMessages.DuplicatedRecord);
        }

        var slug = command.Slug.Slugify();
        var path = $"{product.Category.Slug}/{slug}";

        try
        {
            var picturePath = await _fileUploader.Upload(command.Picture, path, cancellationToken);
            product.Edit(command.Name, command.Code,
                command.ShortDescription, command.Description, picturePath,
                command.PictureAlt, command.PictureTitle, command.CategoryId, slug,
                command.Keywords, command.MetaDescription);

            await _productRepository.SaveChanges(cancellationToken);

            // Log information 
            _logger.LogInformation("Product updated successfully: {ProductName}", command.Name);

            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            // Log error 
            _logger.LogError(ex, "An error occurred during product update: {ErrorMessage}", ex.Message);
            return operation.Failed(ApplicationMessages.ErrorOccurred);
        }
    }

    public async Task<EditProduct> GetDetails(long id,CancellationToken cancellationToken)
    {
        var productDetails = await _productRepository.GetDetails(id, cancellationToken);

        if (productDetails == null)
        {
            // Log warning 
            _logger.LogWarning("No product details found with ID: {ProductId}", id);
            return null!; // or handle the case of not finding the product details as per your application's logic
        }

        // Log information 
        _logger.LogInformation("Retrieved product details successfully for ID: {ProductId}", id);

        return productDetails;
    }

    public async Task<List<ProductViewModel>> GetProducts(CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProducts( cancellationToken);

        // Log information 
        _logger.LogInformation("Retrieved product list successfully.");

        return products;
    }

    public async Task<List<ProductViewModel>> Search(ProductSearchModel searchModel,CancellationToken cancellationToken)
    {
        var products = await _productRepository.Search(searchModel, cancellationToken);

        // Log information 
        _logger.LogInformation("Product search completed successfully.");

        return products;
    }

    public async Task<OperationResult> Activate(long id, CancellationToken cancellationToken)
    {
        var operation = new OperationResult();
        var product = await _productRepository.Get(id, cancellationToken);

        if (product == null)
        {
            // Log warning 
            _logger.LogWarning("No product found with ID: {ProductId}", id);
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        try
        {
            product.Activate();
            await _productRepository.SaveChanges(cancellationToken);

            // Log information 
            _logger.LogInformation("Product activate successfully: {ProductName}", product.Name);

            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            // Log error 
            _logger.LogError(ex, "An error occurred during activating product: {ErrorMessage}", ex.Message);
            return operation.Failed(ApplicationMessages.ErrorOccurred);
        }
    }

    public async Task<OperationResult> DeActive(long id, CancellationToken cancellationToken)
    {
        var operation = new OperationResult();
        var product = await _productRepository.Get(id, cancellationToken);

        if (product == null)
        {
            // Log warning 
            _logger.LogWarning("No product found with ID: {ProductId}", id);
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        try
        {
            product.DeActive();
            await _productRepository.SaveChanges(cancellationToken);

            // Log information 
            _logger.LogInformation("Product deActive successfully: {ProductName}", product.Name);

            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            // Log error 
            _logger.LogError(ex, "An error occurred during deActivating product: {ErrorMessage}", ex.Message);
            return operation.Failed(ApplicationMessages.ErrorOccurred);
        }
    }

    public async Task<long> GetProductSeller(long id, CancellationToken cancellationToken)
    {
        return await _productRepository.GetProductSeller(id,cancellationToken);
    }
}