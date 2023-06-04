using BP._Query.Contracts.Article;
using Microsoft.AspNetCore.Mvc;

namespace WebHost.ViewComponents;

public class LatestArticlesViewComponent:ViewComponent
{
    private readonly IArticleQuery _articleQuery;

    public LatestArticlesViewComponent(IArticleQuery articleQuery)
    {
        _articleQuery = articleQuery;
    }

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
    {
        var articles = await _articleQuery.LatestArticles(cancellationToken);
        return View(articles);

    }
}