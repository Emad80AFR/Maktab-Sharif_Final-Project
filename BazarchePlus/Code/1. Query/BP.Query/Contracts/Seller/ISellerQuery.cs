namespace BP._Query.Contracts.Seller;

public interface ISellerQuery
{
    Task<List<SellerQueryModel>> GetSellers(CancellationToken cancellationToken);
}