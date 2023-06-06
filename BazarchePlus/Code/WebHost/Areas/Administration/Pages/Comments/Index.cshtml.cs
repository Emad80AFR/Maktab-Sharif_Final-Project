using CM._Application.Contracts.Comment;
using CM._Application.Contracts.Comment.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Areas.Administration.Pages.Comments
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public List<CommentViewModel> Comments;
        public CommentSearchModel SearchModel;
        private readonly ICommentApplication _commentApplication;

        public IndexModel(ICommentApplication commentApplication)
        {
            _commentApplication = commentApplication;
        }

        public async Task OnGet(CommentSearchModel searchModel,CancellationToken cancellationToken)
        {
            Comments = await _commentApplication.Search(searchModel,cancellationToken);
        }

        public async Task<IActionResult> OnGetCancel(long id,CancellationToken cancellationToken)
        {
            var result = await _commentApplication.Cancel(id,cancellationToken);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetConfirm(long id,CancellationToken cancellationToken)
        {
            var result = await _commentApplication.Confirm(id,cancellationToken);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }
    }
}
