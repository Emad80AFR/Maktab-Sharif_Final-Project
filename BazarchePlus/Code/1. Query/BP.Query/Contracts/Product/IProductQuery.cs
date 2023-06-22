
using SM._Application.Contracts.Order.DTO_s;

namespace BP._Query.Contracts.Product
{
    public interface IProductQuery
    {
        Task<ProductQueryModel> GetProductDetails(string slug,CancellationToken cancellationToken);
        Task<List<ProductQueryModel>> GetLatestArrivals(CancellationToken cancellationToken);
        Task<List<ProductQueryModel>> Search(string value, CancellationToken cancellationToken);
        Task<List<CartItem>> CheckInventoryStatus(List<CartItem> cartItems,CancellationToken cancellationToken);
    }
}
