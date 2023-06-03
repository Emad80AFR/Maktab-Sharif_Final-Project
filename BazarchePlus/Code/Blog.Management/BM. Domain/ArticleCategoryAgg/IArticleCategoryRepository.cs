using BM._Application.Contracts.ArticleCategory.DTO_s;
using FrameWork.Domain;

namespace BM._Domain.ArticleCategoryAgg;

public interface IArticleCategoryRepository:IBaseRepository<long,ArticleCategory>
{
    Task<string> GetSlugBy(long id,CancellationToken cancellationToken);
    Task<EditArticleCategory> GetDetails(long id, CancellationToken cancellationToken);
    Task<List<ArticleCategoryViewModel>> GetArticleCategories(CancellationToken cancellationToken);
    Task<List<ArticleCategoryViewModel>> Search(ArticleCategorySearchModel searchModel, CancellationToken cancellationToken);


}