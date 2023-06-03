using BM._Application.Contracts.ArticleCategory.DTO_s;

namespace BlogManagement.Application.Contracts.ArticleCategory
{
    public class EditArticleCategory : CreateArticleCategory
    {
        public long Id { get; set; }
    }
}
