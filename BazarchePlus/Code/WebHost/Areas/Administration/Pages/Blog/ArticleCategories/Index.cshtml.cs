using BM._Application.Contracts.ArticleCategory;
using BM._Application.Contracts.ArticleCategory.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Areas.Administration.Pages.Blog.ArticleCategories
{
    public class IndexModel : PageModel
    {
        public ArticleCategorySearchModel SearchModel = null!;
        public List<ArticleCategoryViewModel> ArticleCategories = null!;

        private readonly IArticleCategoryApplication _articleCategoryApplication;

        public IndexModel(IArticleCategoryApplication articleCategoryApplication)
        {
            _articleCategoryApplication = articleCategoryApplication;
        }

        public async Task OnGet(ArticleCategorySearchModel searchModel,CancellationToken cancellationToken)
        {
            ArticleCategories = await _articleCategoryApplication.Search(searchModel,cancellationToken);
        }

        public IActionResult OnGetCreate()
        {
            return Partial("./Create", new CreateArticleCategory());
        }

        public async Task<JsonResult> OnPostCreate(CreateArticleCategory command,CancellationToken cancellationToken)
        {
            var result = await _articleCategoryApplication.Create(command,cancellationToken);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetEdit(long id,CancellationToken cancellationToken)
        {
            var productCategory = await _articleCategoryApplication.GetDetails(id,cancellationToken);
            return Partial("Edit", productCategory);
        }

        public async Task<JsonResult> OnPostEdit(EditArticleCategory command,CancellationToken cancellationToken)
        {
            var result = await _articleCategoryApplication.Edit(command,cancellationToken);
            return new JsonResult(result);
        }
    }
}
