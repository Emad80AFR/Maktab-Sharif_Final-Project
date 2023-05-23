using FrameWork.Application;
using SM._Application.Contracts.ProductCategory.DTO_s;

namespace SM._Application.Contracts.ProductCategory;

public interface IProductCategoryApplication
{
    Task<OperationResult>  Create(CreateProductCategory command);
    Task<OperationResult> Edit(EditProductCategory command);
    Task<EditProductCategory>GetDetails(long id);
    Task<List<ProductCategoryViewModel>> GetProductCategories();
    Task<List<ProductCategoryViewModel>> Search(ProductCategorySearchModel searchModel);

}