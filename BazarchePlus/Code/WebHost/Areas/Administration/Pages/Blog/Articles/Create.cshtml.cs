using BM._Application.Contracts.Article;
using BM._Application.Contracts.Article.DTO_s;
using BM._Application.Contracts.ArticleCategory;
using BM._Infrastructure.Configuration.Permissions;
using FrameWork.Infrastructure.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebHost.Areas.Administration.Pages.Blog.Articles
{
    public class CreateModel : PageModel
    {
        public CreateArticle Command;
        public SelectList ArticleCategories;

        private readonly IArticleApplication _articleApplication;
        private readonly IArticleCategoryApplication _articleCategoryApplication;

        public CreateModel(IArticleApplication articleApplication, IArticleCategoryApplication articleCategoryApplication)
        {
            _articleApplication = articleApplication;
            _articleCategoryApplication = articleCategoryApplication;
        }

        [NeedsPermission(BlogPermissions.CreateArticle)]
        public async Task OnGet(CancellationToken cancellationToken)
        {
            ArticleCategories = new SelectList(await _articleCategoryApplication.GetArticleCategories(cancellationToken), "Id", "Name");
        }

        public async Task<IActionResult> OnPost(CreateArticle command,CancellationToken cancellationToken)
        {
            var result = await _articleApplication.Create(command,cancellationToken);
            return RedirectToPage("./Index");
        }
    }
}
