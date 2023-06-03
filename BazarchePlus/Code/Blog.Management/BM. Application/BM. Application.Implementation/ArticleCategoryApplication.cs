using BM._Application.Contracts.ArticleCategory;
using BM._Application.Contracts.ArticleCategory.DTO_s;
using BM._Domain.ArticleCategoryAgg;
using FrameWork.Application;
using FrameWork.Application.FileOpload;
using FrameWork.Application.Messages;
using Microsoft.Extensions.Logging;

namespace BM._Application.Implementation
{
    public class ArticleCategoryApplication:IArticleCategoryApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly ILogger<ArticleCategoryApplication> _logger;
        private readonly IArticleCategoryRepository _articleCategoryRepository;

        public ArticleCategoryApplication(IFileUploader fileUploader, ILogger<ArticleCategoryApplication> logger, IArticleCategoryRepository articleCategoryRepository)
        {
            _fileUploader = fileUploader;
            _logger = logger;
            _articleCategoryRepository = articleCategoryRepository;
        }

        public async Task<OperationResult> Create(CreateArticleCategory command, CancellationToken cancellationToken)
        {
            var operation = new OperationResult();
            try
            {
                if (await _articleCategoryRepository.Exist(x => x.Name == command.Name, cancellationToken))
                    return operation.Failed(ApplicationMessages.DuplicatedRecord);

                var slug = command.Slug.Slugify();
                var pictureName = await _fileUploader.Upload(command.Picture, slug, cancellationToken);
                var articleCategory = new ArticleCategory(command.Name, pictureName, command.PictureAlt, command.PictureTitle,
                    command.Description, command.ShowOrder, slug, command.Keywords, command.MetaDescription,
                    command.CanonicalAddress);

                await _articleCategoryRepository.Create(articleCategory,cancellationToken);
                await _articleCategoryRepository.SaveChanges(cancellationToken);

                _logger.LogInformation("Create method executed successfully.");

                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Create method.");
                throw;
            }
        }

        public async Task<OperationResult> Edit(EditArticleCategory command, CancellationToken cancellationToken)
        {
            var operation = new OperationResult();
            try
            {
                var articleCategory = await _articleCategoryRepository.Get(command.Id, cancellationToken);

                if (articleCategory == null)
                    return operation.Failed(ApplicationMessages.RecordNotFound);

                if (await _articleCategoryRepository.Exist(x => x.Name == command.Name && x.Id != command.Id, cancellationToken))
                    return operation.Failed(ApplicationMessages.DuplicatedRecord);

                var slug = command.Slug.Slugify();
                var pictureName = await _fileUploader.Upload(command.Picture, slug, cancellationToken);
                articleCategory.Edit(command.Name, pictureName, command.PictureAlt, command.PictureTitle,
                    command.Description, command.ShowOrder, slug, command.Keywords, command.MetaDescription,
                    command.CanonicalAddress);

                await _articleCategoryRepository.SaveChanges(cancellationToken);

                _logger.LogInformation("Edit method executed successfully.");

                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Edit method.");
                throw;
            }
        }

        public async Task<EditArticleCategory> GetDetails(long id, CancellationToken cancellationToken)
        {
            try
            {
                var articleCategory = await _articleCategoryRepository.GetDetails(id, cancellationToken);

                _logger.LogInformation("Article category details retrieved successfully.");

                return articleCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving article category details.");

                throw;
            }
        }

        public async Task<List<ArticleCategoryViewModel>> GetArticleCategories(CancellationToken cancellationToken)
        {
            try
            {
                var articleCategories = await _articleCategoryRepository.GetArticleCategories(cancellationToken);

                _logger.LogInformation("Article categories retrieved successfully.");

                return articleCategories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving article categories.");

                throw;
            }
        }

        public async Task<List<ArticleCategoryViewModel>> Search(ArticleCategorySearchModel searchModel, CancellationToken cancellationToken)
        {
            try
            {
                var results = await _articleCategoryRepository.Search(searchModel, cancellationToken);

                _logger.LogInformation("Article category search completed successfully.");

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during article category search.");

                throw;
            }
        }
    }
}