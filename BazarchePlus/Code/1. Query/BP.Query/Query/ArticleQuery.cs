using BM._Infrastructure.EFCore;
using BP._Query.Contracts.Article;
using BP._Query.Contracts.Comment;
using CM._Infrastructure.EFCore;
using FrameWork.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BP._Query.Query;

public class ArticleQuery:IArticleQuery
{
    private readonly ILogger<ArticleQuery> _logger;
    private readonly BlogContext _blogContext;
    private readonly CommentContext _commentContext;

    public ArticleQuery(ILogger<ArticleQuery> logger, BlogContext blogContext, CommentContext commentContext)
    {
        _logger = logger;
        _blogContext = blogContext;
        _commentContext = commentContext;
    }

    public async Task<List<ArticleQueryModel>> LatestArticles(CancellationToken cancellationToken)
    {
        try
        {
            var currentDate = DateTime.Now;

            var articles = await _blogContext.Articles
                .Include(x => x.Category)
                .Where(x => x.PublishDate <= currentDate)
                .Select(x => new ArticleQueryModel
                {
                    Title = x.Title,
                    Slug = x.Slug,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    PublishDate = x.PublishDate.ToFarsi(),
                    ShortDescription = x.ShortDescription,
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation("LatestArticles method executed successfully.");

            return articles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the LatestArticles method.");

            throw; 
        }
    }

    public async Task<ArticleQueryModel> GetArticleDetails(string slug, CancellationToken cancellationToken)
    {
        try
        {
            var currentDate = DateTime.Now;

            var article = await _blogContext.Articles
                .Include(x => x.Category)
                .Where(x => x.PublishDate <= currentDate)
                .Select(x => new ArticleQueryModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    CategoryName = x.Category.Name,
                    CategorySlug = x.Category.Slug,
                    Slug = x.Slug,
                    CanonicalAddress = x.CanonicalAddress,
                    Description = x.Description,
                    Keywords = x.Keywords,
                    MetaDescription = x.MetaDescription,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    PublishDate = x.PublishDate.ToFarsi(),
                    ShortDescription = x.ShortDescription,
                })
                .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);

            if (article != null && !string.IsNullOrWhiteSpace(article.Keywords))
                article.KeywordList = article.Keywords.Split(",").ToList();

            var comments = await _commentContext.Comments
                .Where(x => !x.IsCanceled)
                .Where(x => x.IsConfirmed)
                .Where(x => x.Type == CommentType.Article)
                .Where(x => x.OwnerRecordId == article.Id)
                .Select(x => new CommentQueryModel
                {
                    Id = x.Id,
                    Message = x.Message,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    CreationDate = x.CreationDate.ToFarsi()
                })
                .OrderByDescending(x => x.Id)
                .ToListAsync(cancellationToken);

            foreach (var comment in comments)
            {
                if (comment.ParentId > 0)
                    comment.parentName = comments.FirstOrDefault(x => x.Id == comment.ParentId)?.Name!;
            }

            article!.Comments = comments;

            _logger.LogInformation("GetArticleDetails method executed successfully.");

            return article!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the GetArticleDetails method.");

            throw; 
        }
    }
}