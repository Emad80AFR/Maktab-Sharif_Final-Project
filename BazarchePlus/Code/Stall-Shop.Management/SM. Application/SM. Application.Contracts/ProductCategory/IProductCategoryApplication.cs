using FrameWork.Application;
using SM._Application.Contracts.ProductCategory.DTO_s;

namespace SM._Application.Contracts.ProductCategory;

public interface IProductCategoryApplication
{
    Task<OperationResult>  Create(CreateProductCategory command, CancellationToken cancellationToken);
    Task<OperationResult> Edit(EditProductCategory command, CancellationToken cancellationToken);
    Task<EditProductCategory>GetDetails(long id, CancellationToken cancellationToken);
    Task<List<ProductCategoryViewModel>> GetProductCategories(CancellationToken cancellationToken);
    Task<List<ProductCategoryViewModel>>  Search(ProductCategorySearchModel searchModel, CancellationToken cancellationToken);

}