using AM._Application.Contracts.Account;
using AM._Application.Contracts.Account.DTO_s;
using AM._Application.Contracts.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Areas.Administration.Pages.Accounts
{
    public class ProfileModel : PageModel
    {
        public EditAccount Command { get; set; }
        public ChangePassword ChangePassword { get; set; }
        private readonly IRoleApplication _roleApplication;
        private readonly IAccountApplication _accountApplication;

        public ProfileModel(IAccountApplication accountApplication, IRoleApplication roleApplication)
        {
            _roleApplication = roleApplication;
            _accountApplication = accountApplication;
        }
        public async Task OnGet(long id, CancellationToken cancellationToken)
        {
            Command = await _accountApplication.GetDetails(id, cancellationToken);
            Command.Roles = await _roleApplication.List(cancellationToken);
            ChangePassword = new ChangePassword { Id = id };
        }
        public async Task<IActionResult> OnPost(EditAccount command, CancellationToken cancellationToken)
        {
            var result = await _accountApplication.Edit(command, cancellationToken);
            return RedirectToPage("/Index");
        }
        
        public async Task<IActionResult> OnPostChangePassword(ChangePassword command, CancellationToken cancellationToken)
        {
            var result = await _accountApplication.ChangePassword(command, cancellationToken);
            return RedirectToPage("/Index");
        }
    }
}
