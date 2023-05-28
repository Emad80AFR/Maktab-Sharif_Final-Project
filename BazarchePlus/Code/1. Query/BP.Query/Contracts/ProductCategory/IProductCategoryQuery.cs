namespace BP._Query.Contracts.ProductCategory
{
    public interface IProductCategoryQuery
    {
        //Task<ProductCategoryQueryModel> GetProductCategoryWithProductsBy(string slug);
        Task<List<ProductCategoryQueryModel>> GetProductCategories(CancellationToken cancellationToken);
        //Task<List<ProductCategoryQueryModel>> GetProductCategoriesWithProducts();
    }
}
