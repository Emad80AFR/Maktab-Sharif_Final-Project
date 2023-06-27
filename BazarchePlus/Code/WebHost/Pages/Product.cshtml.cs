using BP._Query.Contracts.Product;
using CM._Application.Contracts.Comment;
using CM._Application.Contracts.Comment.DTO_s;
using CM._Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Pages
{
    public class ProductModel : PageModel
    {
        [TempData]
        public string ErrorMessage { get; set; }

        public ProductQueryModel Product;
        private readonly IProductQuery _productQuery;
        private readonly ICommentApplication _commentApplication;

        public ProductModel(IProductQuery productQuery, ICommentApplication commentApplication)
        {
            _productQuery = productQuery;
            _commentApplication = commentApplication;
        }

        public async Task OnGet(string? message,string id,CancellationToken cancellationToken)
        {
            if (message != null)
                ErrorMessage = message; 

            Product = await _productQuery.GetProductDetails(id,cancellationToken);
        }

        public async Task<IActionResult> OnPost(AddComment command, string productSlug ,CancellationToken cancellationToken)
        {
            command.Type = CommentType.Product;
            var result = await _commentApplication.Add(command,cancellationToken);
            return
                !result.IsSucceeded ? RedirectToPage("/Product", new { Message = result.Message , Id = productSlug }) 
                : RedirectToPage("/Product", new { Id = productSlug });
        }
    }
}
