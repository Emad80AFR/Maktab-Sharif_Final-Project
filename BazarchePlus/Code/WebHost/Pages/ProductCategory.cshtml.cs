using BP._Query.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Pages
{
    public class ProductCategoryModel : PageModel
    {
        public ProductCategoryQueryModel ProductCategory;
        private readonly IProductCategoryQuery _productCategoryQuery;

        public ProductCategoryModel(IProductCategoryQuery productCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
        }

        public async Task OnGet(string id,CancellationToken cancellationToken)
        {
            ProductCategory = await _productCategoryQuery.GetProductCategoryWithProductsBy(id,cancellationToken);
        }
    }
}
