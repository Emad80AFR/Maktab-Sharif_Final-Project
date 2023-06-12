using FrameWork.Infrastructure.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SM._Application.Contracts.Slide;
using SM._Application.Contracts.Slide.DTO_s;
using SM._Infrastructure.Configuration.Permissions;

namespace WebHost.Areas.Administration.Pages.Shop.Slides
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public List<SlideViewModel> Slides;

        private readonly ISlideApplication _slideApplication;

        public IndexModel(ISlideApplication slideApplication)
        {
            _slideApplication = slideApplication;
        }

        [NeedsPermission(ShopPermissions.ListSlide)]
        public async Task OnGet(CancellationToken cancellationToken)
        {
            Slides = await _slideApplication.GetList(cancellationToken);
        }

        [NeedsPermission(ShopPermissions.CreateSlide)]
        public IActionResult OnGetCreate()
        {
            var command = new CreateSlide();
            return Partial("./Create", command);
        }

        public async Task<JsonResult> OnPostCreate(CreateSlide command,CancellationToken cancellationToken)
        {
            var result = await _slideApplication.Create(command, cancellationToken);
            return new JsonResult(result);
        }

        [NeedsPermission(ShopPermissions.EditSlide)]
        public async Task<IActionResult> OnGetEdit(long id,CancellationToken cancellationToken)
        {
            var slide = await _slideApplication.GetDetails(id, cancellationToken);
            return Partial("Edit", slide);
        }

        public async Task<JsonResult> OnPostEdit(EditSlide command,CancellationToken cancellationToken)
        {
            var result = await _slideApplication.Edit(command, cancellationToken);
            return new JsonResult(result);
        }

        [NeedsPermission(ShopPermissions.DeleteSlide)]
        public async Task<IActionResult> OnGetRemove(long id,CancellationToken cancellationToken)
        {
            var result = await _slideApplication.Remove(id, cancellationToken);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }

        [NeedsPermission(ShopPermissions.RestoreSlide)]
        public async Task<IActionResult> OnGetRestore(long id,CancellationToken cancellationToken)
        {
            var result = await _slideApplication.Restore(id, cancellationToken);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }
    }
}
