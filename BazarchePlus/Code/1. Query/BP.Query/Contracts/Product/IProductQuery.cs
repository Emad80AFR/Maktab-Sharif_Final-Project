
namespace BP._Query.Contracts.Product
{
    public interface IProductQuery
    {
        Task<ProductQueryModel> GetProductDetails(string slug,CancellationToken cancellationToken);
        Task<List<ProductQueryModel>> GetLatestArrivals(CancellationToken cancellationToken);
        Task<List<ProductQueryModel>> Search(string value, CancellationToken cancellationToken);
        //List<CartItem> CheckInventoryStatus(List<CartItem> cartItems);
    }
}
