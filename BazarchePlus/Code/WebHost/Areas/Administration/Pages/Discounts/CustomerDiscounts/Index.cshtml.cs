using DM._Application.Contracts.CustomerDiscount.DTO_s;
using DM._Infrastructure.Configuration.Permissions;
using DM.Application.Contracts.CustomerDiscount;
using DM.Application.Contracts.CustomerDiscount.DTO_s;
using FrameWork.Infrastructure.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SM._Application.Contracts.Product;

namespace WebHost.Areas.Administration.Pages.Discounts.CustomerDiscounts
{
    //[Authorize(Roles = Roles.Administator)]
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public CustomerDiscountSearchModel SearchModel;
        public List<CustomerDiscountViewModel> CustomerDiscounts;
        public SelectList Products;

        private readonly IProductApplication _productApplication;
        private readonly ICustomerDiscountApplication _customerDiscountApplication;

        public IndexModel(IProductApplication productApplication, ICustomerDiscountApplication customerDiscountApplication)
        {
            _productApplication = productApplication;
            _customerDiscountApplication = customerDiscountApplication;
        }

        [NeedsPermission(DiscountsPermissions.ListCustomerDiscounts)]
        public async Task OnGet(CustomerDiscountSearchModel searchModel,CancellationToken cancellationToken)
        {
            Products = new SelectList(await _productApplication.GetProducts(cancellationToken), "Id", "Name");
            CustomerDiscounts = await _customerDiscountApplication.Search(searchModel,cancellationToken);
        }

        [NeedsPermission(DiscountsPermissions.DefineCustomerDiscounts)]
        public async Task<IActionResult> OnGetCreate(CancellationToken cancellationToken)
        {
            var command = new DefineCustomerDiscount
            {
                Products = await _productApplication.GetProducts(cancellationToken)
            };
            return Partial("./Create", command);
        }

        public async Task<JsonResult> OnPostCreate(DefineCustomerDiscount command,CancellationToken cancellationToken)
        {
            var result = await _customerDiscountApplication.Define(command,cancellationToken);
            return new JsonResult(result);
        }

        [NeedsPermission(DiscountsPermissions.EditCustomerDiscount)]
        public async Task<IActionResult> OnGetEdit(long id,CancellationToken cancellationToken)
        {
            var customerDiscount = await _customerDiscountApplication.GetDetails(id,cancellationToken);
            customerDiscount.Products = await _productApplication.GetProducts(cancellationToken);
            return Partial("Edit", customerDiscount);
        }

        public async Task<JsonResult> OnPostEdit(EditCustomerDiscount command,CancellationToken cancellationToken)
        {
            var result = await _customerDiscountApplication.Edit(command,cancellationToken);
            return new JsonResult(result);
        }
    }
}
