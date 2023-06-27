using FrameWork.Application;
using SM._Application.Contracts.Product.DTO_s;

namespace SM._Application.Contracts.Product;

public interface IProductApplication
{
    Task<OperationResult> Create(CreateProduct command,CancellationToken cancellationToken);
    Task<OperationResult> Edit(EditProduct command, CancellationToken cancellationToken);
    Task<EditProduct> GetDetails(long id, CancellationToken cancellationToken);
    Task<List<ProductViewModel>> GetProducts(CancellationToken cancellationToken);
    Task<List<ProductViewModel>> Search(ProductSearchModel searchModel, CancellationToken cancellationToken);
    Task<OperationResult> Activate(long id, CancellationToken cancellationToken);
    Task<OperationResult> DeActive(long id, CancellationToken cancellationToken);
    Task<long> GetProductSeller(long id, CancellationToken cancellationToken);

}