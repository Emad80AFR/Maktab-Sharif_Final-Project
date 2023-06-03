using BM._Application.Contracts.Article.DTO_s;
using BM._Domain.ArticleAgg;
using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BM._Infrastructure.EFCore.Repository;

public class ArticleRepository:BaseRepository<long,Article>,IArticleRepository
{
    private readonly ILogger<ArticleRepository> _logger;
    private readonly BlogContext _blogContext;

    public ArticleRepository(BlogContext blogContext, ILogger<ArticleRepository> logger):base(blogContext,logger)
    {
        _blogContext = blogContext;
        _logger = logger;
    }

    public async Task<EditArticle> GetDetails(long id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _blogContext.Articles.Select(x => new EditArticle
            {
                Id = x.Id,
                CanonicalAddress = x.CanonicalAddress,
                CategoryId = x.CategoryId,
                Description = x.Description,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsi(),
                ShortDescription = x.ShortDescription,
                Slug = x.Slug,
                Title = x.Title
            }).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (result == null)
            {
                _logger.LogWarning("Article details not found for ID: {Id}", id);
            }
            else
            {
                _logger.LogInformation("Article details retrieved successfully for ID: {Id}", id);
            }

            return result!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving article details for ID: {Id}", id);

            throw;
        }
    }

    public async Task<Article> GetWithCategory(long id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _blogContext.Articles
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (result == null)
            {
                _logger.LogWarning("Article not found for ID: {Id}", id);
            }
            else
            {
                _logger.LogInformation("Article retrieved successfully for ID: {Id}", id);
            }

            return result!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving article for ID: {Id}", id);

            throw;
        }
    }

    public async Task<List<ArticleViewModel>> Search(ArticleSearchModel searchModel, CancellationToken cnaCancellationToken)
    {
        try
        {
            var query = _blogContext.Articles.Select(x => new ArticleViewModel
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Category = x.Category.Name,
                Picture = x.Picture,
                PublishDate = x.PublishDate.ToFarsi(),
                ShortDescription = x.ShortDescription.Substring(0, Math.Min(x.ShortDescription.Length, 50)) + " ...",
                Title = x.Title
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Title))
                query = query.Where(x => x.Title.Contains(searchModel.Title));

            if (searchModel.CategoryId > 0)
                query = query.Where(x => x.CategoryId == searchModel.CategoryId);

            var results = await query.OrderByDescending(x => x.Id).ToListAsync(cnaCancellationToken);

            _logger.LogInformation("Article search completed successfully");

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during article search");

            throw;
        }
    }
}