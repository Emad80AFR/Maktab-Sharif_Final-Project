using AM._Application.Contracts.Auction;
using AM._Application.Contracts.Auction.DTO_s;
using AM._Infrastructure.Configuration.Permissions;
using FrameWork.Infrastructure.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SM._Application.Contracts.Product;

namespace WebHost.Areas.Administration.Pages.Auctions.Auction
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public AuctionSearchModel SearchModel;
        public List<AuctionViewModel> Auctions;
        public SelectList Products;

        private readonly IProductApplication _productApplication;
        private readonly IAuctionApplication _auctionApplication;
        public IndexModel(IProductApplication productApplication, IAuctionApplication auctionApplication)
        {
            _productApplication = productApplication;
            _auctionApplication= auctionApplication;
        }

        [NeedsPermission(AuctionsPermissions.ListAuctions)]
        public async Task OnGet(AuctionSearchModel searchModel,CancellationToken cancellationToken)
        {
            Products = new SelectList( await _productApplication.GetProducts(cancellationToken), "Id", "Name");
            Auctions = await _auctionApplication.Search(searchModel, cancellationToken);
        }

        [NeedsPermission(AuctionsPermissions.DefineAuction)]
        public async Task<IActionResult> OnGetCreate(CancellationToken cancellationToken)
        {
            var command = new DefineAuction
            {
                Products = await _productApplication.GetProducts(cancellationToken)
            };
            return Partial("./Create", command);
        }

        public async Task<JsonResult> OnPostCreate(DefineAuction command,CancellationToken cancellationToken)
        {
            var result = await _auctionApplication.Define(command, cancellationToken);
            return new JsonResult(result);
        }

        [NeedsPermission(AuctionsPermissions.EditAuction)]
        public async Task<IActionResult> OnGetEdit(long id,CancellationToken cancellationToken)
        {
            var auction =await _auctionApplication.GetDetails(id, cancellationToken);
            auction.Products =await _productApplication.GetProducts(cancellationToken);
            return Partial("Edit", auction);
        }

        public async Task<JsonResult> OnPostEdit(EditAuction command,CancellationToken cancellationToken)
        {
            var result = await _auctionApplication.Edit(command, cancellationToken);
            return new JsonResult(result);
        }

        [NeedsPermission(AuctionsPermissions.DeActiveAuction)]
        public async Task<IActionResult> OnGetDeActive(long id,CancellationToken cancellationToken)
        {
            var result = await _auctionApplication.DeActive(id, cancellationToken);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }

        [NeedsPermission(AuctionsPermissions.ActivateAuction)]
        public async Task<IActionResult> OnGetActivate(long id,CancellationToken cancellationToken)
        {
            var result = await _auctionApplication.Activate(id, cancellationToken);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }
    }
}
