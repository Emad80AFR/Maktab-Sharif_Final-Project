using BP._Query.Contracts.Product;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Pages
{
    public class SearchModel : PageModel
    {
        public string Value;
        public List<ProductQueryModel> Products;
        private readonly IProductQuery _productQuery;

        public SearchModel(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public async Task OnGet(string value,CancellationToken cancellationToken)
        {
            Value = value;
            Products = await _productQuery.Search(value,cancellationToken);
        }
    }
}
