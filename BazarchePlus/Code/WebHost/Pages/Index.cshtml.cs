using AM._Application.Contracts.Auction;
using AM._Application.Contracts.Auction.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IAuctionService _auctionService;

        public IndexModel(ILogger<IndexModel> logger, IAuctionService auctionService)
        {
            _logger = logger;
            _auctionService = auctionService;
        }

        public void OnGet()
        {

        }

        public void OnPostAddBid(Bid bid,long productId)
        {
            _auctionService.AddBid(bid, productId);
        }
    }
}