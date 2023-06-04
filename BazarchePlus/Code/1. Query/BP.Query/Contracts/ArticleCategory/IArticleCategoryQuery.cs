namespace BP._Query.Contracts.ArticleCategory
{
    public interface IArticleCategoryQuery
    {
        Task<ArticleCategoryQueryModel> GetArticleCategory(string slug, CancellationToken cancellationToken);
        Task<List<ArticleCategoryQueryModel>> GetArticleCategories(CancellationToken cancellationToken);
    }
}
