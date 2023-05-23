using FrameWork.Domain;
using SM._Application.Contracts.ProductCategory.DTO_s;

namespace SM._Domain.ProductCategoryAgg;

public interface IProductCategoryRepository:IBaseRepository<long,ProductCategory>
{
    Task<List<ProductCategoryViewModel>> GetProductCategories();
    Task<EditProductCategory?> GetDetails(long id);
    Task<string> GetSlugById(long id);
    Task<List<ProductCategoryViewModel>> Search(ProductCategorySearchModel searchModel);

}