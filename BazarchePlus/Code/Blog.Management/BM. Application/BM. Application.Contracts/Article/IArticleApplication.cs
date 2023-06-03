using BM._Application.Contracts.Article.DTO_s;
using FrameWork.Application;

namespace BM._Application.Contracts.Article;

public interface IArticleApplication
{
    Task<OperationResult> Create(CreateArticle command,CancellationToken cancellationToken);
    Task<OperationResult> Edit(EditArticle command,CancellationToken cancellationToken);
    Task<EditArticle> GetDetails(long id,CancellationToken cancellationToken);
    Task<List<ArticleViewModel>> Search(ArticleSearchModel searchModel, CancellationToken cancellationToken);

}