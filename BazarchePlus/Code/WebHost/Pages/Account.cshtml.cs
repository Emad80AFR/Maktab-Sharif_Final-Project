using AM._Application.Contracts.Account;
using AM._Application.Contracts.Account.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebHost.Pages
{
    public class AccountModel : PageModel
    {
        [TempData]
        public string LoginMessage { get; set; }
        [TempData]
        public string RegisterMessage { get; set; }


        private readonly IAccountApplication _accountApplication;

        public AccountModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }

        public void OnGet(string? message)
        {
            LoginMessage = message!;
        }

        public async Task<IActionResult> OnPostLogin(Login command,CancellationToken cancellationToken)
        {
            var result = await _accountApplication.Login(command,cancellationToken);

            return result.IsSucceeded ? RedirectToPage("/Index") : RedirectToPage("./Account", new { message = result.Message });

            //LoginMessage = result.Message;
            //return RedirectToPage("./Account");

            //ModelState.AddModelError("loginError",result.Message);
            //return Page();
        }

        public async Task<IActionResult> OnGetLogout(CancellationToken cancellationToken)
        {
            await _accountApplication.Logout(cancellationToken);
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostRegister(RegisterAccount command,CancellationToken cancellationToken)
        { var result = await _accountApplication.Register(command,cancellationToken);
            if (result.IsSucceeded)
                return RedirectToPage("/Account");
            RegisterMessage = result.Message;
            return RedirectToPage("/Account");
        }
    }
}
