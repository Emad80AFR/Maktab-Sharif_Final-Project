using FrameWork.Domain;
using SM._Application.Contracts.Product.DTO_s;

namespace SM._Domain.ProductAgg;

public interface IProductRepository:IBaseRepository<long,Product>
{
    Task<EditProduct> GetDetails(long id);
    Task<Product> GetProductWithCategory(long id);
    Task<List<ProductViewModel>> GetProducts();
    Task<List<ProductViewModel>> Search(ProductSearchModel searchModel);

}