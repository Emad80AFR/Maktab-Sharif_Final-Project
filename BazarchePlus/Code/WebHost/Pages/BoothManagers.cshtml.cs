using BP._Query.Contracts.ProductCategory;
using BP._Query.Contracts.Seller;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Pages
{
    public class BoothManagersModel : PageModel
    {
        public List<SellerQueryModel> Sellers { get; set; }
        private readonly ISellerQuery _sellerQuery;
        public BoothManagersModel(ISellerQuery sellerQuery)
        {
            _sellerQuery = sellerQuery;
            Sellers=new List<SellerQueryModel>();
        }

        public async Task OnGet(CancellationToken cancellationToken)
        {
            Sellers = await _sellerQuery.GetSellers(cancellationToken);
        }
    }
}
