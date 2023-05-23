using AM._Application.Contracts.Account;
using AM._Application.Contracts.Account.DTO_s;
using FrameWork.Application;

namespace AM._Application.Implementation
{
    public class AccountApplication:IAccountApplication
    {
        public Task<AccountViewModel> GetAccountBy(long id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Register(RegisterAccount command)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Edit(EditAccount command)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> ChangePassword(ChangePassword command)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Login(Login command)
        {
            throw new NotImplementedException();
        }

        public Task<EditAccount> GetDetails(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountViewModel>> Search(AccountSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountViewModel>> GetAccounts()
        {
            throw new NotImplementedException();
        }
    }
}