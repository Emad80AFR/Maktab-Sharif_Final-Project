using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SM._Application.Contracts.Product;
using SM._Application.Contracts.ProductPicture;
using SM._Application.Contracts.ProductPicture.DTO_s;
using FrameWork.Infrastructure.Permission;
using SM._Infrastructure.Configuration.Permissions;

namespace WebHost.Areas.Administration.Pages.Shop.ProductPictures
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public ProductPictureSearchModel SearchModel;
        public List<ProductPictureViewModel> ProductPictures;
        public SelectList Products;

        private readonly IProductApplication _productApplication;
        private readonly IProductPictureApplication _productPictureApplication;
        public IndexModel(IProductApplication productApplication, IProductPictureApplication productPictureApplication)
        {
            _productApplication = productApplication;
            _productPictureApplication = productPictureApplication;
        }

        [NeedsPermission(ShopPermissions.ListProductPictures)]
        public async Task OnGet(ProductPictureSearchModel searchModel,CancellationToken cancellationToken)
        {
            Products = new SelectList( await _productApplication.GetProducts(cancellationToken), "Id", "Name");
            ProductPictures = await _productPictureApplication.Search(searchModel, cancellationToken);
        }

        [NeedsPermission(ShopPermissions.CreateProductPicture)]
        public async Task<IActionResult> OnGetCreate(CancellationToken cancellationToken)
        {
            var command = new CreateProductPicture
            {
                Products = await _productApplication.GetProducts(cancellationToken)
            };
            return Partial("./Create", command);
        }

        public async Task<JsonResult> OnPostCreate(CreateProductPicture command,CancellationToken cancellationToken)
        {
            var result = await _productPictureApplication.Create(command, cancellationToken);
            return new JsonResult(result);
        }

        [NeedsPermission(ShopPermissions.EditProductPicture)]
        public async Task<IActionResult> OnGetEdit(long id,CancellationToken cancellationToken)
        {
            var productPicture =await _productPictureApplication.GetDetails(id, cancellationToken);
            productPicture.Products =await _productApplication.GetProducts(cancellationToken);
            return Partial("Edit", productPicture);
        }

        public async Task<JsonResult> OnPostEdit(EditProductPicture command,CancellationToken cancellationToken)
        {
            var result = await _productPictureApplication.Edit(command, cancellationToken);
            return new JsonResult(result);
        }

        [NeedsPermission(ShopPermissions.DeleteProductPicture)]
        public async Task<IActionResult> OnGetRemove(long id,CancellationToken cancellationToken)
        {
            var result = await _productPictureApplication.Remove(id, cancellationToken);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }

        [NeedsPermission(ShopPermissions.RestoreProductPicture)]
        public async Task<IActionResult> OnGetRestore(long id,CancellationToken cancellationToken)
        {
            var result = await _productPictureApplication.Restore(id, cancellationToken);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }
    }
}
