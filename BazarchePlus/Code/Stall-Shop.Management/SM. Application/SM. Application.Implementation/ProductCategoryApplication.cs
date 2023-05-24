using FrameWork.Application;
using Microsoft.Extensions.Logging;
using SM._Application.Contracts.ProductCategory;
using SM._Application.Contracts.ProductCategory.DTO_s;
using SM._Domain.ProductCategoryAgg;

namespace SM._Application.Implementation;

public class ProductCategoryApplication:IProductCategoryApplication
{
    private readonly IFileUploader _fileUploader;
    private readonly ILogger<ProductCategoryApplication> _logger;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public ProductCategoryApplication(IProductCategoryRepository productCategoryRepository, IFileUploader fileUploader,ILogger<ProductCategoryApplication> logger)
    {
        _productCategoryRepository = productCategoryRepository;
        _fileUploader = fileUploader;
        _logger = logger;
    }

    public async Task<OperationResult> Create(CreateProductCategory command)
    {
        var operation = new OperationResult();
        if (await _productCategoryRepository.Exist(x => x.Name == command.Name))
        {
            // Log warning 
            _logger.LogWarning("Duplicated record detected.");
            return operation.Failed(ApplicationMessages.DuplicatedRecord);
        }

        var slug = command.Slug.Slugify();

        var picturePath = $"{command.Slug}";
        var pictureName = await _fileUploader.Upload(command.Picture, picturePath);

        var productCategory = new ProductCategory(command.Name, command.Description,
            pictureName, command.PictureAlt, command.PictureTitle, command.Keywords,
            command.MetaDescription, slug);

        await _productCategoryRepository.Create(productCategory);
        await _productCategoryRepository.SaveChanges();

        // Log information 
        _logger.LogInformation("Product category creation completed successfully.");

        return operation.Succeeded();
    }

    public async Task<OperationResult> Edit(EditProductCategory command)
    {
        var operation = new OperationResult();
        var productCategory = await _productCategoryRepository.Get(command.Id);

        if (productCategory == null)
        {
            // Log warning 
            _logger.LogWarning("Record not found.");
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        if (await _productCategoryRepository.Exist(x => x.Name == command.Name && x.Id != command.Id))
        {
            // Log warning 
            _logger.LogWarning("Duplicated record detected.");
            return operation.Failed(ApplicationMessages.DuplicatedRecord);
        }

        var slug = command.Slug.Slugify();

        var picturePath = $"{command.Slug}";
        var fileName = await _fileUploader.Upload(command.Picture, picturePath);

        productCategory.Edit(command.Name, command.Description, fileName,
            command.PictureAlt, command.PictureTitle, command.Keywords,
            command.MetaDescription, slug);

        await _productCategoryRepository.SaveChanges();

        // Log information 
        _logger.LogInformation("Product category update completed successfully.");

        return operation.Succeeded();
    }

    public async Task<EditProductCategory> GetDetails(long id)
    {
        var productCategoryDetails = await _productCategoryRepository.GetDetails(id);

        if (productCategoryDetails == null)
        {
            // Log warning 
            _logger.LogWarning("Product category details not found.");
            return null!;
        }

        // Log information 
        _logger.LogInformation("Retrieved product category details successfully.");

        return productCategoryDetails;
    }

    public  Task<List<ProductCategoryViewModel>> GetProductCategories()
    {
        var productCategories =  _productCategoryRepository.GetProductCategories();

        // Log information 
        _logger.LogInformation("Retrieved product categories successfully.");

        return productCategories; ;
    }

    public async Task<List<ProductCategoryViewModel>> Search(ProductCategorySearchModel searchModel)
    {
        var productCategories = await _productCategoryRepository.Search(searchModel);

        // Log information 
        _logger.LogInformation("Product category search completed successfully.");

        return productCategories;
    }
}