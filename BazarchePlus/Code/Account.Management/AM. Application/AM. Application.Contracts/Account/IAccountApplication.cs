using AM._Application.Contracts.Account.DTO_s;
using FrameWork.Application;

namespace AM._Application.Contracts.Account;

public interface IAccountApplication
{
    Task<AccountViewModel> GetAccountBy(long id);
    Task<OperationResult> Register(RegisterAccount command);
    Task<OperationResult> Edit(EditAccount command);
    Task<OperationResult> ChangePassword(ChangePassword command);
    Task<OperationResult> Login(Login command);
    Task<EditAccount> GetDetails(long id);
    Task<List<AccountViewModel>> Search(AccountSearchModel searchModel);
    Task Logout();
    Task<List<AccountViewModel>> GetAccounts();

}