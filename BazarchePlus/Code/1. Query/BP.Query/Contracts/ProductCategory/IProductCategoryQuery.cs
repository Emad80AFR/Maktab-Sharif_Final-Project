namespace BP._Query.Contracts.ProductCategory
{
    public interface IProductCategoryQuery
    {
        Task<ProductCategoryQueryModel> GetProductCategoryWithProductsBy(string slug,CancellationToken cancellationToken);
        Task<List<ProductCategoryQueryModel>> GetProductCategories(CancellationToken cancellationToken);
        Task<List<ProductCategoryQueryModel>> GetProductCategoriesWithProducts(CancellationToken cancellationToken);
    }
}
