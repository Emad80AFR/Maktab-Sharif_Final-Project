using AM._Application.Contracts.Role;
using AM._Application.Contracts.Role.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;

namespace WebHost.Areas.Administration.Pages.Accounts.Role
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public List<RoleViewModel> Roles;

        private readonly IRoleApplication _roleApplication;

        public IndexModel(IRoleApplication roleApplication)
        {
            _roleApplication = roleApplication;
        }

        public IActionResult OnGetCreate()
        {
            var command = new CreateRole();
            return Partial(".Create", command);
        }

        public async Task<JsonResult> OnPostCreate(CreateRole command,CancellationToken cancellationToken)
        {
            var result = await _roleApplication.Create(command,cancellationToken);
            return new JsonResult(result);

        }
        public async Task OnGet(CancellationToken cancellationToken)
        {
            Roles = await _roleApplication.List(cancellationToken);
        }

        public async Task<IActionResult> OnGetEdit(long id,CancellationToken cancellationToken)
        {
            var role = await _roleApplication.GetDetails(id, cancellationToken);
            return Partial("Edit", role);
        }

        public async Task<JsonResult> OnPostEdit(EditRole command,CancellationToken cancellationToken)
        {
            var result = await _roleApplication.Edit(command, cancellationToken);
            return new JsonResult(result);
        }
    }
}
