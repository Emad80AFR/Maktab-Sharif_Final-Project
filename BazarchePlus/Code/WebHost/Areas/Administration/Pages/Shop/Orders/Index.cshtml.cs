using AM._Application.Contracts.Account;
using FrameWork.Infrastructure.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SM._Application.Contracts.Order;
using SM._Application.Contracts.Order.DTO_s;
using SM._Infrastructure.Configuration.Permissions;

namespace WebHost.Areas.Administration.Pages.Shop.Orders
{
    public class IndexModel : PageModel
    {
        public OrderSearchModel SearchModel;
        public SelectList Accounts;
        public List<OrderViewModel> Orders;

        private readonly IOrderApplication _orderApplication;
        private readonly IAccountApplication _accountApplication;

        public IndexModel(IOrderApplication orderApplication, IAccountApplication accountApplication)
        {
            _orderApplication = orderApplication;
            _accountApplication = accountApplication;
        }

        [NeedsPermission(ShopPermissions.SeeOrder)]
        public async Task OnGet(OrderSearchModel searchModel,CancellationToken cancellationToken)
        {
            Accounts = new SelectList(await _accountApplication.GetAccounts(cancellationToken), "Id", "Fullname");
            Orders = await _orderApplication.Search(searchModel,cancellationToken);
        }

        [NeedsPermission(ShopPermissions.ConfirmOrder)]
        public async Task<IActionResult> OnGetConfirm(long id,CancellationToken cancellationToken)
        {
             await _orderApplication.PaymentSucceeded(id, 0,cancellationToken);
            return RedirectToPage("./Index");
        }

        [NeedsPermission(ShopPermissions.RejectOrder)]
        public async Task<IActionResult> OnGetCancel(long id,CancellationToken cancellationToken)
        {
            await _orderApplication.Cancel(id,cancellationToken);
            return RedirectToPage("./Index");
        }

        [NeedsPermission(ShopPermissions.SeeOrderItems)]
        public async Task<IActionResult> OnGetItems(long id,CancellationToken cancellationToken)
        {
            var items = await _orderApplication.GetItems(id,cancellationToken);
            return Partial("Items", items);
        }
    }
}