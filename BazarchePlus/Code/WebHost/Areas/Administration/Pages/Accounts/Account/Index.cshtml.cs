using AM._Application.Contracts.Account;
using AM._Application.Contracts.Account.DTO_s;
using AM._Application.Contracts.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebHost.Areas.Administration.Pages.Accounts.Account
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public AccountSearchModel SearchModel;
        public List<AccountViewModel> Accounts;
        public SelectList Roles;

        private readonly IRoleApplication _roleApplication;
        private readonly IAccountApplication _accountApplication;

        public IndexModel(IAccountApplication accountApplication, IRoleApplication roleApplication)
        {
            _roleApplication = roleApplication;
            _accountApplication = accountApplication;
        }

        public async Task OnGet(AccountSearchModel searchModel,CancellationToken cancellationToken)
        {
            Roles = new SelectList(await _roleApplication.List(cancellationToken), "Id", "Name");
            Accounts = await _accountApplication.Search(searchModel,cancellationToken);
        }

        public async Task<IActionResult> OnGetCreate(CancellationToken cancellationToken)
        {
            var command = new RegisterAccount
            {
                Roles = await _roleApplication.List(cancellationToken)
            };
            return Partial("./Create", command);
        }

        public async Task<JsonResult> OnPostCreate(RegisterAccount command,CancellationToken cancellationToken)
        {
            var result = await _accountApplication.Register(command,cancellationToken);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetEdit(long id,CancellationToken cancellationToken)
        {
            var account = await _accountApplication.GetDetails(id,cancellationToken);
            account.Roles = await _roleApplication.List(cancellationToken);
            return Partial("Edit", account);
        }

        public async Task<JsonResult> OnPostEdit(EditAccount command,CancellationToken cancellationToken)
        {
            var result = await _accountApplication.Edit(command,cancellationToken);
            return new JsonResult(result);
        }

        public IActionResult OnGetChangePassword(long id)
        {
            var command = new ChangePassword { Id = id };
            return Partial("ChangePassword", command);
        }

        public async Task<JsonResult> OnPostChangePassword(ChangePassword command,CancellationToken cancellationToken)
        {
            var result = await _accountApplication.ChangePassword(command,cancellationToken);
            return new JsonResult(result);
        }
    }
}
