using BM._Application.Contracts.ArticleCategory.DTO_s;
using BM._Domain.ArticleCategoryAgg;
using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BM._Infrastructure.EFCore.Repository;

public class ArticleCategoryRepository:BaseRepository<long,ArticleCategory>,IArticleCategoryRepository
{

    private readonly ILogger<ArticleCategoryRepository> _logger;
    private readonly BlogContext _blogContext;

    public ArticleCategoryRepository(BlogContext blogContext, ILogger<ArticleCategoryRepository> logger):base(blogContext,logger)
    {
        _blogContext = blogContext;
        _logger = logger;
    }

    public async Task<string> GetSlugBy(long id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _blogContext.ArticleCategories
                .Where(x => x.Id == id)
                .Select(x => x.Slug)
                .FirstOrDefaultAsync(cancellationToken);

            _logger.LogInformation("Retrieved article category slug successfully.");

            return result!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving article category slug.");

            throw;
        }
    }

    public async Task<EditArticleCategory> GetDetails(long id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _blogContext.ArticleCategories
                .Where(x => x.Id == id)
                .Select(x => new EditArticleCategory
                {
                    Id = x.Id,
                    Name = x.Name,
                    CanonicalAddress = x.CanonicalAddress,
                    Description = x.Description,
                    Keywords = x.Keywords,
                    MetaDescription = x.MetaDescription,
                    ShowOrder = x.ShowOrder,
                    Slug = x.Slug,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle
                })
                .FirstOrDefaultAsync(cancellationToken);

            _logger.LogInformation("Retrieved article category details successfully.");

            return result!;
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
            var results = await _blogContext.ArticleCategories
                .Select(x => new ArticleCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved article categories successfully.");

            return results;
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
            var query = _blogContext.ArticleCategories
                .Include(x => x.Articles)
                .Select(x => new ArticleCategoryViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Picture = x.Picture,
                    ShowOrder = x.ShowOrder,
                    CreationDate = x.CreationDate.ToFarsi(),
                    ArticlesCount = x.Articles.Count
                });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            var result = await query.OrderByDescending(x => x.ShowOrder).ToListAsync(cancellationToken);

            _logger.LogInformation("Article categories search completed successfully.");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during article categories search.");

            throw;
        }
    }
}