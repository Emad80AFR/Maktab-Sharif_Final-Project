using BM._Application.Contracts.Article;
using BM._Application.Contracts.Article.DTO_s;
using BM._Application.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebHost.Areas.Administration.Pages.Blog.Articles
{
    public class IndexModel : PageModel
    {
        public ArticleSearchModel SearchModel;
        public List<ArticleViewModel> Articles;
        public SelectList ArticleCategories;

        private readonly IArticleApplication _articleApplication;
        private readonly IArticleCategoryApplication _articleCategoryApplication;

        public IndexModel(IArticleApplication articleApplication, IArticleCategoryApplication articleCategoryApplication)
        {
            _articleApplication = articleApplication;
            _articleCategoryApplication = articleCategoryApplication;
        }

        public async Task OnGet(ArticleSearchModel searchModel,CancellationToken cancellationToken)
        {
            ArticleCategories = new SelectList(await _articleCategoryApplication.GetArticleCategories(cancellationToken), "Id", "Name");
            Articles = await _articleApplication.Search(searchModel,cancellationToken);
        }
    }
}
