using BP._Query.Contracts.Article;
using BP._Query.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Pages
{
    public class ArticleCategoryModel : PageModel
    {
        public ArticleCategoryQueryModel ArticleCategory;
        public List<ArticleCategoryQueryModel> ArticleCategories;
        public List<ArticleQueryModel> LatestArticles;

        private readonly IArticleQuery _articleQuery;
        private readonly IArticleCategoryQuery _articleCategoryQuery;

        public ArticleCategoryModel(IArticleCategoryQuery articleCategoryQuery, IArticleQuery articleQuery)
        {
            _articleQuery = articleQuery;
            _articleCategoryQuery = articleCategoryQuery;
        }

        public async Task OnGet(string id,CancellationToken cancellationToken)
        {
            LatestArticles = await _articleQuery.LatestArticles(cancellationToken);
            ArticleCategory = await _articleCategoryQuery.GetArticleCategory(id,cancellationToken);
            ArticleCategories = await _articleCategoryQuery.GetArticleCategories(cancellationToken);
        }
    }
}
