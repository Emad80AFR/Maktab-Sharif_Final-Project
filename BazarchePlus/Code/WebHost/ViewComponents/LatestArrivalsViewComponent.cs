using BP._Query.Contracts.Product;
using Microsoft.AspNetCore.Mvc;

namespace WebHost.ViewComponents;

public class LatestArrivalsViewComponent:ViewComponent
{
    private readonly IProductQuery _productQuery;

    public LatestArrivalsViewComponent(IProductQuery productQuery)
    {
        _productQuery = productQuery;
    }

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
    {
        var products = await _productQuery.GetLatestArrivals(cancellationToken);
        return View(products);

    }
}