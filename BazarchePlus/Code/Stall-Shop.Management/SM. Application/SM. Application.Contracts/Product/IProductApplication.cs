using FrameWork.Application;
using SM._Application.Contracts.Product.DTO_s;

namespace SM._Application.Contracts.Product;

public interface IProductApplication
{
    Task<OperationResult> Create(CreateProduct command);
    Task<OperationResult> Edit(EditProduct command);
    Task<EditProduct> GetDetails(long id);
    Task<List<ProductViewModel>> GetProducts();
    Task<List<ProductViewModel>> Search(ProductSearchModel searchModel);

}