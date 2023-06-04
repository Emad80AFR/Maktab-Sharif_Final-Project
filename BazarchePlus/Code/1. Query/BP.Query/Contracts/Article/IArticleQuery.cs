namespace BP._Query.Contracts.Article
{
    public interface IArticleQuery
    {
        Task<List<ArticleQueryModel>> LatestArticles(CancellationToken cancellationToken);
        Task<ArticleQueryModel> GetArticleDetails(string slug,CancellationToken cancellationToken);
    }
}
