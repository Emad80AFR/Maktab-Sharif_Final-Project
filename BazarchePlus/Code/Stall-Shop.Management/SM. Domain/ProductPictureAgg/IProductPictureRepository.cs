using FrameWork.Domain;
using SM._Application.Contracts.ProductPicture.DTO_s;

namespace SM._Domain.ProductPictureAgg;

public interface IProductPictureRepository:IBaseRepository<long,ProductPicture>
{
    Task<EditProductPicture> GetDetails(long id);
    Task<ProductPicture> GetWithProductAndCategory(long id);
    Task<List<ProductPictureViewModel>> Search(ProductPictureSearchModel searchModel);

}