using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SM._Application.Contracts.ProductCategory;
using SM._Application.Contracts.ProductCategory.DTO_s;
using System.Threading;

namespace WebHost.Areas.Administration.Pages.Shop.ProductCategories
{
    public class IndexModel : PageModel
    {
        public ProductCategorySearchModel SearchModel;
        public List<ProductCategoryViewModel> ProductCategories;

        private readonly IProductCategoryApplication _productCategoryApplication;

        public IndexModel(IProductCategoryApplication productCategoryApplication)
        {
            _productCategoryApplication = productCategoryApplication;
        }

        //[NeedsPermission(ShopPermissions.ListProductCategories)]
        public async Task OnGet(ProductCategorySearchModel searchModel,CancellationToken cancellationToken)
        {
            ProductCategories = await _productCategoryApplication.Search(searchModel, cancellationToken);
        }

        public IActionResult OnGetCreate()
        {
            return Partial("./Create", new CreateProductCategory());
        }

        //[NeedsPermission(ShopPermissions.CreateProductCategory)]
        public JsonResult OnPostCreate(CreateProductCategory command,CancellationToken cancellationToken)
        {
            var result = _productCategoryApplication.Create(command, cancellationToken);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id, CancellationToken cancellationToken)
        {
            var productCategory = _productCategoryApplication.GetDetails(id, cancellationToken);
            return Partial("Edit", productCategory);
        }

        //[NeedsPermission(ShopPermissions.EditProductCategory)]
        public JsonResult OnPostEdit(EditProductCategory command, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
            }

            var result = _productCategoryApplication.Edit(command, cancellationToken);
            return new JsonResult(result);
        }
    }
}