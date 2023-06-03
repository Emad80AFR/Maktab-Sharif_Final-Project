using BM._Application.Contracts.Article.DTO_s;
using FrameWork.Domain;

namespace BM._Domain.ArticleAgg;

public interface IArticleRepository:IBaseRepository<long,Article>
{
    Task<EditArticle> GetDetails(long id,CancellationToken cancellationToken);
    Task<Article> GetWithCategory(long id,CancellationToken cancellationToken);
    Task<List<ArticleViewModel>> Search(ArticleSearchModel searchModel,CancellationToken cnaCancellationToken);

}