using FrameWork.Application;
using Microsoft.Extensions.Logging;
using SM._Application.Contracts.Product;
using SM._Application.Contracts.Product.DTO_s;
using SM._Domain.ProductAgg;
using SM._Domain.ProductCategoryAgg;

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

    public async Task<OperationResult> Create(CreateProduct command)
    {
        var operation = new OperationResult();

        if (await _productRepository.Exist(x => x.Name == command.Name))
        {
            // Log error if needed
            _logger.LogError("Product creation failed due to duplicated record: {ProductName}", command.Name);
            return operation.Failed(ApplicationMessages.DuplicatedRecord);
        }

        var slug = command.Slug.Slugify();
        var categorySlug = await _productCategoryRepository.GetSlugById(command.CategoryId);
        var path = $"{categorySlug}//{slug}";

        try
        {
            var picturePath = await _fileUploader.Upload(command.Picture, path);
            var product = new Product(command.Name, command.Code,
                command.ShortDescription, command.Description, picturePath,
                command.PictureAlt, command.PictureTitle, command.CategoryId, slug,
                command.Keywords, command.MetaDescription);

            await _productRepository.Create(product);
            await _productRepository.SaveChanges();

            // Log information if needed
            _logger.LogInformation("Product created successfully: {ProductName}", command.Name);

            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            // Log error if needed
            _logger.LogError(ex, "An error occurred during product creation: {ErrorMessage}", ex.Message);
            return operation.Failed(ApplicationMessages.ErrorOccurred);
        }
    }

    public async Task<OperationResult> Edit(EditProduct command)
    {
        var operation = new OperationResult();
        var product = await _productRepository.GetProductWithCategory(command.Id);

        if (product == null)
        {
            // Log warning if needed
            _logger.LogWarning("No product found with ID: {ProductId}", command.Id);
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        if (await _productRepository.Exist(x => x.Name == command.Name && x.Id != command.Id))
        {
            // Log warning if needed
            _logger.LogWarning("Product creation failed due to duplicated record: {ProductName}", command.Name);
            return operation.Failed(ApplicationMessages.DuplicatedRecord);
        }

        var slug = command.Slug.Slugify();
        var path = $"{product.Category.Slug}/{slug}";

        try
        {
            var picturePath = await _fileUploader.Upload(command.Picture, path);
            product.Edit(command.Name, command.Code,
                command.ShortDescription, command.Description, picturePath,
                command.PictureAlt, command.PictureTitle, command.CategoryId, slug,
                command.Keywords, command.MetaDescription);

            await _productRepository.SaveChanges();

            // Log information if needed
            _logger.LogInformation("Product updated successfully: {ProductName}", command.Name);

            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            // Log error if needed
            _logger.LogError(ex, "An error occurred during product update: {ErrorMessage}", ex.Message);
            return operation.Failed(ApplicationMessages.ErrorOccurred);
        }
    }

    public async Task<EditProduct> GetDetails(long id)
    {
        var productDetails = await _productRepository.GetDetails(id);

        if (productDetails == null)
        {
            // Log warning if needed
            _logger.LogWarning("No product details found with ID: {ProductId}", id);
            return null!; // or handle the case of not finding the product details as per your application's logic
        }

        // Log information if needed
        _logger.LogInformation("Retrieved product details successfully for ID: {ProductId}", id);

        return productDetails;
    }

    public async Task<List<ProductViewModel>> GetProducts()
    {
        var products = await _productRepository.GetProducts();

        // Log information if needed
        _logger.LogInformation("Retrieved product list successfully.");

        return products;
    }

    public async Task<List<ProductViewModel>> Search(ProductSearchModel searchModel)
    {
        var products = await _productRepository.Search(searchModel);

        // Log information if needed
        _logger.LogInformation("Product search completed successfully.");

        return products;
    }
}