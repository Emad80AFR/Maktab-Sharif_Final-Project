using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SM._Application.Contracts.Product;
using SM._Application.Contracts.Product.DTO_s;
using SM._Application.Contracts.ProductCategory;
using FrameWork.Infrastructure;
using FrameWork.Infrastructure.Permission;
using SM._Infrastructure.Configuration.Permissions;

namespace WebHost.Areas.Administration.Pages.Shop.Products
{
    public class IndexModel : PageModel
    {
        [TempData] public string Message { get; set; }
        public ProductSearchModel SearchModel;
        public List<ProductViewModel> Products;
        public SelectList ProductCategories;

        private readonly IProductApplication _productApplication;
        private readonly IProductCategoryApplication _productCategoryApplication;

        public IndexModel(IProductApplication productApplication,
            IProductCategoryApplication productCategoryApplication)
        {
            _productApplication = productApplication;
            _productCategoryApplication = productCategoryApplication;
        }

        [NeedsPermission(ShopPermissions.ListProducts)]
        public async Task OnGet(ProductSearchModel searchModel,CancellationToken cancellationToken)
        {
            ProductCategories = new SelectList( await _productCategoryApplication.GetProductCategories(cancellationToken), "Id", "Name");
            Products =await _productApplication.Search(searchModel, cancellationToken);
        }

        [NeedsPermission(ShopPermissions.CreateProduct)]
        public async Task<IActionResult> OnGetCreate(CancellationToken cancellationToken)
        {
            var command = new CreateProduct
            {
                Categories = await _productCategoryApplication.GetProductCategories(cancellationToken)
            };
            return Partial("./Create", command);
        }

        public async Task<JsonResult> OnPostCreate(CreateProduct command,CancellationToken cancellationToken)
        {
            var result = await _productApplication.Create(command, cancellationToken);
            return new JsonResult(result);
        }

        [NeedsPermission(ShopPermissions.EditProduct)]
        public async Task<IActionResult> OnGetEdit(long id,CancellationToken cancellationToken)
        {
            var product =await _productApplication.GetDetails(id, cancellationToken);
            product.Categories =await _productCategoryApplication.GetProductCategories(cancellationToken);
            return Partial("Edit", product);
        }

        public async Task<JsonResult> OnPostEdit(EditProduct command, CancellationToken cancellationToken)
        {
            var result = await _productApplication.Edit(command, cancellationToken);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetActivate(long id, CancellationToken cancellationToken)
        {
            var result = await _productApplication.Activate(id, cancellationToken);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");
            Message = result.Message;
            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnGetDeActivate(long id, CancellationToken cancellationToken)
        {
            var result = await _productApplication.DeActive(id, cancellationToken);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");
            Message = result.Message;
            return RedirectToPage("./Index");
        }
    }
}