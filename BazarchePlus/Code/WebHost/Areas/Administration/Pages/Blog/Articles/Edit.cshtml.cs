using BM._Application.Contracts.Article;
using BM._Application.Contracts.Article.DTO_s;
using BM._Application.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebHost.Areas.Administration.Pages.Blog.Articles
{
    public class EditModel : PageModel
    {
        public EditArticle Command;
        public SelectList ArticleCategories;

        private readonly IArticleApplication _articleApplication;
        private readonly IArticleCategoryApplication _articleCategoryApplication;

        public EditModel(IArticleApplication articleApplication, IArticleCategoryApplication articleCategoryApplication)
        {
            _articleApplication = articleApplication;
            _articleCategoryApplication = articleCategoryApplication;
        }

        public async Task OnGet(long id,CancellationToken cancellationToken)
        {
            Command = await _articleApplication.GetDetails(id,cancellationToken);
            ArticleCategories = new SelectList(await _articleCategoryApplication.GetArticleCategories(cancellationToken), "Id", "Name");
        }

        public async Task<IActionResult> OnPost(EditArticle command,CancellationToken cancellationToken)
        {
            var result =await _articleApplication.Edit(command,cancellationToken);
            return RedirectToPage("./Index");
        }
    }
}
