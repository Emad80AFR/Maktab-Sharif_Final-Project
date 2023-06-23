using BP._Query.Contracts.Product;
using BP._Query.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Pages
{
    public class SellersProductsModel : PageModel
    {
        public List<ProductQueryModel> Products { get; set; }
        private readonly IProductQuery _productQuery;

        public SellersProductsModel(IProductQuery productQuery)
        {
            _productQuery = productQuery;
            Products = new List<ProductQueryModel>();
        }

        public async Task OnGet(long sellerId,CancellationToken cancellationToken)
        {
            Products = await _productQuery.GetProductsBy(sellerId,cancellationToken);
        }
    }
}
