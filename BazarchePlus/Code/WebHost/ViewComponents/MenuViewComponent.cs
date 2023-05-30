using BP._Query;
using BP._Query.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace WebHost.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryQuery _productCategoryQuery;
        public MenuViewComponent(IProductCategoryQuery productCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            var result = new MenuModel
            {
                ProductCategories = await _productCategoryQuery.GetProductCategories(cancellationToken)
            };
            return View(result);
        }
    }
}
