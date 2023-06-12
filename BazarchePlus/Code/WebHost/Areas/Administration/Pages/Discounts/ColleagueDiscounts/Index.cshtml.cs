using DM._Infrastructure.Configuration.Permissions;
using DM.Application.Contracts.ColleagueDiscount;
using DM.Application.Contracts.ColleagueDiscount.DTO_s;
using FrameWork.Infrastructure.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SM._Application.Contracts.Product;

namespace WebHost.Areas.Administration.Pages.Discounts.ColleagueDiscounts
{
    //[Authorize(Roles = Roles.Administator)]
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public ColleagueDiscountSearchModel SearchModel;
        public List<ColleagueDiscountViewModel> ColleagueDiscounts;
        public SelectList Products;

        private readonly IProductApplication _productApplication;
        private readonly IColleagueDiscountApplication _colleagueDiscountApplication;

        public IndexModel(IProductApplication productApplication, IColleagueDiscountApplication colleagueDiscountApplication)
        {
            _productApplication = productApplication;
            _colleagueDiscountApplication = colleagueDiscountApplication;
        }

        [NeedsPermission(DiscountsPermissions.ListColleagueDiscounts)]
        public async Task OnGet(ColleagueDiscountSearchModel searchModel,CancellationToken cancellationToken)
        {
            Products = new SelectList(await _productApplication.GetProducts(cancellationToken), "Id", "Name");
            ColleagueDiscounts = await _colleagueDiscountApplication.Search(searchModel,cancellationToken);
        }

        [NeedsPermission(DiscountsPermissions.DefineColleagueDiscounts)]
        public async Task<IActionResult> OnGetCreate(CancellationToken cancellationToken)
        {
            var command = new DefineColleagueDiscount
            {
                Products = await _productApplication.GetProducts(cancellationToken)
            };
            return Partial("./Create", command);
        }

        public async Task<JsonResult> OnPostCreate(DefineColleagueDiscount command,CancellationToken cancellationToken)
        {
            var result = await _colleagueDiscountApplication.Define(command,cancellationToken);
            return new JsonResult(result);
        }

        [NeedsPermission(DiscountsPermissions.EditColleagueDiscount)]
        public async Task<IActionResult> OnGetEdit(long id,CancellationToken cancellationToken)
        {
            var colleagueDiscount = await _colleagueDiscountApplication.GetDetails(id,cancellationToken);
            colleagueDiscount.Products = await _productApplication.GetProducts(cancellationToken);
            return Partial("Edit", colleagueDiscount);
        }

        public async Task<JsonResult> OnPostEdit(EditColleagueDiscount command,CancellationToken cancellationToken)
        {
            var result = await _colleagueDiscountApplication.Edit(command,cancellationToken);
            return new JsonResult(result);
        }

        [NeedsPermission(DiscountsPermissions.DeActiveColleagueDiscount)]
        public async Task<IActionResult> OnGetRemove(long id,CancellationToken cancellationToken)
        {
            await _colleagueDiscountApplication.Remove(id,cancellationToken);
            return RedirectToPage("./Index");
        }

        [NeedsPermission(DiscountsPermissions.ActiveColleagueDiscount)]
        public async Task<IActionResult> OnGetRestore(long id,CancellationToken cancellationToken)
        {
            await _colleagueDiscountApplication.Restore(id,cancellationToken);
            return RedirectToPage("./Index");
        }
    }
}
