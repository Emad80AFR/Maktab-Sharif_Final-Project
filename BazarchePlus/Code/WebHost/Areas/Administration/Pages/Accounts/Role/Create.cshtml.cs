using AM._Application.Contracts.Role;
using AM._Application.Contracts.Role.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Areas.Administration.Pages.Accounts.Role
{
    public class CreateModel : PageModel
    {
        public CreateRole Command;
        private readonly IRoleApplication _roleApplication;

        public CreateModel(IRoleApplication roleApplication)
        {
            _roleApplication = roleApplication;
        }

        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPost(CreateRole command,CancellationToken cancellationToken)
        {
            var result = await _roleApplication.Create(command,cancellationToken);
            return RedirectToPage("Index");
        }
    }
}