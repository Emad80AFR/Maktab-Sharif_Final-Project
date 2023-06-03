using BM._Application.Contracts.Article;
using BM._Application.Contracts.Article.DTO_s;
using BM._Domain.ArticleAgg;
using BM._Domain.ArticleCategoryAgg;
using FrameWork.Application;
using FrameWork.Application.FileOpload;
using FrameWork.Application.Messages;
using Microsoft.Extensions.Logging;

namespace BM._Application.Implementation;

public class ArticleApplication:IArticleApplication
{
    private readonly ILogger<ArticleApplication> _logger;
    private readonly IArticleRepository _articleRepository;
    private readonly IArticleCategoryRepository _articleCategoryRepository;
    private readonly IFileUploader _fileUploader;
    public ArticleApplication(ILogger<ArticleApplication> logger, IArticleRepository articleRepository, IFileUploader fileUploader, IArticleCategoryRepository articleCategoryRepository)
    {
        _logger = logger;
        _articleRepository = articleRepository;
        _fileUploader = fileUploader;
        _articleCategoryRepository = articleCategoryRepository;
    }

    public async Task<OperationResult> Create(CreateArticle command, CancellationToken cancellationToken)
    {
        try
        {
            var operation = new OperationResult();
            if (await _articleRepository.Exist(x => x.Title == command.Title, cancellationToken))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var categorySlug = await _articleCategoryRepository.GetSlugBy(command.CategoryId, cancellationToken);
            var path = $"{categorySlug}/{slug}";
            var pictureName = await _fileUploader.Upload(command.Picture, path, cancellationToken);
            var publishDate = command.PublishDate.ToGeorgianDateTime();

            var article = new Article(command.Title, command.ShortDescription, command.Description, pictureName,
                command.PictureAlt, command.PictureTitle, publishDate, slug, command.Keywords, command.MetaDescription,
                command.CanonicalAddress, command.CategoryId);

            await _articleRepository.Create(article, cancellationToken);
            await _articleRepository.SaveChanges(cancellationToken);
            _logger.LogInformation("Create method executed successfully.");
            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during article creation");

            throw;
        }
    }

    public async Task<OperationResult> Edit(EditArticle command, CancellationToken cancellationToken)
    {
        try
        {
            var operation = new OperationResult();
            var article = await _articleRepository.GetWithCategory(command.Id, cancellationToken);

            if (article == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (await _articleRepository.Exist(x => x.Title == command.Title && x.Id != command.Id, cancellationToken))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var path = $"{article.Category.Slug}/{slug}";
            var pictureName = await _fileUploader.Upload(command.Picture, path, cancellationToken);
            var publishDate = command.PublishDate.ToGeorgianDateTime();

            article.Edit(command.Title, command.ShortDescription, command.Description, pictureName,
                command.PictureAlt, command.PictureTitle, publishDate, slug, command.Keywords, command.MetaDescription,
                command.CanonicalAddress, command.CategoryId);

            await _articleRepository.SaveChanges(cancellationToken);
            _logger.LogInformation("Edit method executed successfully.");
            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during article editing");

            throw;
        }
    }

    public async Task<EditArticle> GetDetails(long id, CancellationToken cancellationToken)
    {
        try
        {
            return await _articleRepository.GetDetails(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving article details");

            throw;
        }
    }

    public async Task<List<ArticleViewModel>> Search(ArticleSearchModel searchModel, CancellationToken cancellationToken)
    {
        try
        {
            return await _articleRepository.Search(searchModel, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching articles");

            throw;
        }
    }
}