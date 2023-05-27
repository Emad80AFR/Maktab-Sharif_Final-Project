using FrameWork.Application;
using SM._Application.Contracts.ProductPicture.DTO_s;

namespace SM._Application.Contracts.ProductPicture;

public interface IProductPictureApplication
{
    Task<OperationResult> Create(CreateProductPicture command, CancellationToken cancellationToken);
    Task<OperationResult> Edit(EditProductPicture command, CancellationToken cancellationToken);
    Task<OperationResult> Remove(long id, CancellationToken cancellationToken);
    Task<OperationResult> Restore(long id, CancellationToken cancellationToken);
    Task<EditProductPicture> GetDetails(long id, CancellationToken cancellationToken);
    Task<List<ProductPictureViewModel>> Search(ProductPictureSearchModel searchModel, CancellationToken cancellationToken);

}