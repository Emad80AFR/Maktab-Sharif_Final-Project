using BM._Application.Contracts.ArticleCategory.DTO_s;
using FrameWork.Application;

namespace BM._Application.Contracts.ArticleCategory;

public interface IArticleCategoryApplication
{
    Task<OperationResult> Create(CreateArticleCategory command,CancellationToken cancellationToken);
    Task<OperationResult> Edit(EditArticleCategory command,CancellationToken cancellationToken);
    Task<EditArticleCategory> GetDetails(long id,CancellationToken cancellationToken);
    Task<List<ArticleCategoryViewModel>> GetArticleCategories(CancellationToken cancellationToken);
    Task<List<ArticleCategoryViewModel>> Search(ArticleCategorySearchModel searchModel,CancellationToken cancellationToken);

}