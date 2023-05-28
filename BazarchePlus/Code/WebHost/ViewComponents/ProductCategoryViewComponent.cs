using BP._Query.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace WebHost.ViewComponents;

public class ProductCategoryViewComponent:ViewComponent
{
    private readonly IProductCategoryQuery _query;

    public ProductCategoryViewComponent(IProductCategoryQuery query)
    {
        _query = query;
    }

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
    {
      
        var productCategories = await _query.GetProductCategories(cancellationToken);
        return View(productCategories);
    }
}