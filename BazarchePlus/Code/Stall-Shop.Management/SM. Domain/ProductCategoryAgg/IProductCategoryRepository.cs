using FrameWork.Domain;
using SM._Application.Contracts.ProductCategory.DTO_s;

namespace SM._Domain.ProductCategoryAgg;

public interface IProductCategoryRepository:IBaseRepository<long,ProductCategory>
{
    Task<List<ProductCategoryViewModel>> GetProductCategories(CancellationToken cancellationToken);
    Task<EditProductCategory?> GetDetails(long id, CancellationToken cancellationToken);
    Task<string> GetSlugById(long id, CancellationToken cancellationToken);
    Task<List<ProductCategoryViewModel>> Search(ProductCategorySearchModel searchModel, CancellationToken cancellationToken);

}