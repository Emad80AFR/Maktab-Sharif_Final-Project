using BM._Domain.ArticleAgg;
using BM._Infrastructure.EFCore;
using BP._Query.Contracts.Article;
using BP._Query.Contracts.ArticleCategory;
using FrameWork.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BP._Query.Query;

public class ArticleCategoryQuery:IArticleCategoryQuery
{
    private readonly ILogger<ArticleCategoryQuery> _logger;
    private readonly BlogContext _blogContext;

    public ArticleCategoryQuery(ILogger<ArticleCategoryQuery> logger, BlogContext blogContext)
    {
        _logger = logger;
        _blogContext = blogContext;
    }

    public async Task<ArticleCategoryQueryModel> GetArticleCategory(string slug, CancellationToken cancellationToken)
    {
        try
        {
            var articleCategory = await _blogContext.ArticleCategories
                .Include(x => x.Articles)
                .Select(x => new ArticleCategoryQueryModel
                {
                    Slug = x.Slug,
                    Name = x.Name,
                    Description = x.Description,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Keywords = x.Keywords,
                    MetaDescription = x.MetaDescription,
                    CanonicalAddress = x.CanonicalAddress,
                    ArticlesCount = x.Articles.Count,
                    Articles = MapArticles(x.Articles)
                })
                .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);

            if (!string.IsNullOrWhiteSpace(articleCategory!.Keywords))
                articleCategory.KeywordList = articleCategory.Keywords.Split(",").ToList();

            _logger.LogInformation("GetArticleCategory method executed successfully.");

            return articleCategory;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the GetArticleCategory method.");

            throw; 
        }
    }
    private static List<ArticleQueryModel> MapArticles(List<Article> articles)
    {
        return articles.Select(x => new ArticleQueryModel
        {
            Slug = x.Slug,
            ShortDescription = x.ShortDescription,
            Title = x.Title,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            PublishDate = x.PublishDate.ToFarsi(),
        }).ToList();
    }

    public async Task<List<ArticleCategoryQueryModel>> GetArticleCategories(CancellationToken cancellationToken)
    {
        try
        {
            var categories = await _blogContext.ArticleCategories
                .Include(x => x.Articles)
                .Select(x => new ArticleCategoryQueryModel
                {
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug,
                    ArticlesCount = x.Articles.Count
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation("GetArticleCategories method executed successfully.");

            return categories;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the GetArticleCategories method.");

            throw; 
        }
    }
}