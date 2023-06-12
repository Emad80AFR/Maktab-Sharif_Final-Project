using BM._Application.Contracts.Article;
using BM._Application.Contracts.Article.DTO_s;
using BM._Application.Contracts.ArticleCategory;
using BM._Infrastructure.Configuration.Permissions;
using FrameWork.Infrastructure.Permission;
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

        [NeedsPermission(BlogPermissions.ListArticle)]
        public async Task OnGet(ArticleSearchModel searchModel,CancellationToken cancellationToken)
        {
            ArticleCategories = new SelectList(await _articleCategoryApplication.GetArticleCategories(cancellationToken), "Id", "Name");
            Articles = await _articleApplication.Search(searchModel,cancellationToken);
        }
    }
}
