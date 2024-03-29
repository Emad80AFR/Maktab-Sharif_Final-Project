using BP._Query.Contracts.Article;
using BP._Query.Contracts.ArticleCategory;
using CM._Application.Contracts.Comment;
using CM._Application.Contracts.Comment.DTO_s;
using CM._Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Pages
{
    public class ArticleModel : PageModel
    {
        public ArticleQueryModel Article;
        public List<ArticleQueryModel> LatestArticles;
        public List<ArticleCategoryQueryModel> ArticleCategories;
        private readonly IArticleQuery _articleQuery;
        private readonly IArticleCategoryQuery _articleCategoryQuery;
        private readonly ICommentApplication _commentApplication;

        public ArticleModel(IArticleQuery articleQuery, IArticleCategoryQuery articleCategoryQuery, ICommentApplication commentApplication)
        {
            _articleQuery = articleQuery;
            _commentApplication = commentApplication;
            _articleCategoryQuery = articleCategoryQuery;
        }

        public async Task OnGet(string id,CancellationToken cancellationToken)
        {
            Article = await _articleQuery.GetArticleDetails(id,cancellationToken);
            LatestArticles = await _articleQuery.LatestArticles(cancellationToken);
            ArticleCategories = await _articleCategoryQuery.GetArticleCategories(cancellationToken);
        }

        public async Task<IActionResult> OnPost(AddComment command, string articleSlug,CancellationToken cancellationToken)
        {
            command.Type = CommentType.Article;
            var result = await _commentApplication.Add(command,cancellationToken);
            return RedirectToPage("/Article", new { Id = articleSlug });
        }
    }
}
