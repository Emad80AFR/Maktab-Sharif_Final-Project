using BP._Query.Contracts.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Pages
{
    public class ProductModel : PageModel
    {
        public ProductQueryModel Product;
        private readonly IProductQuery _productQuery;
        //private readonly ICommentApplication _commentApplication;

        public ProductModel(IProductQuery productQuery/*, ICommentApplication commentApplication*/)
        {
            _productQuery = productQuery;
            //_commentApplication = commentApplication;
        }

        public async Task OnGet(string id,CancellationToken cancellationToken)
        {
            Product = await _productQuery.GetProductDetails(id,cancellationToken);
        }

        //public IActionResult OnPost(AddComment command, string productSlug)
        //{
        //    command.Type = CommentType.Product;
        //    var result = _commentApplication.Add(command);
        //    return RedirectToPage("/Product", new { Id = productSlug });
        //}
    }
}
