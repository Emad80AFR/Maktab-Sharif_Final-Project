using BP._Query.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace WebHost.ViewComponents
{
    public class AuctionsProductsViewComponent : ViewComponent
    {
        private readonly IProductCategoryQuery _productCategoryQuery;

        public AuctionsProductsViewComponent(IProductCategoryQuery productCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            var categories =await _productCategoryQuery.GetAuctionProducts(cancellationToken);
            return View(categories);
        }
    }
}
