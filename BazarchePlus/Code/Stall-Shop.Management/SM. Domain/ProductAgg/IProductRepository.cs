using FrameWork.Domain;
using SM._Application.Contracts.Product.DTO_s;

namespace SM._Domain.ProductAgg;

public interface IProductRepository:IBaseRepository<long,Product>
{
    Task<EditProduct> GetDetails(long id, CancellationToken cancellationToken);
    Task<Product> GetProductWithCategory(long id, CancellationToken cancellationToken);
    Task<List<ProductViewModel>> GetProducts(CancellationToken cancellationToken);
    Task<List<ProductViewModel>> Search(ProductSearchModel searchModel, CancellationToken cancellationToken);
    Task<long> GetProductSeller(long id, CancellationToken cancellationToken);

}