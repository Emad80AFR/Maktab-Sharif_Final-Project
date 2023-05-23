using AM._Application.Contracts.Account.DTO_s;
using FrameWork.Domain;

namespace AM._Domain.AccountAgg;

public interface IAccountRepository:IBaseRepository<long,Account>
{
    Task<Account> GetBy(string username);
    Task<EditAccount> GetDetails(long id);
    Task<List<AccountViewModel>> GetAccounts();
    Task<List<AccountViewModel>> Search(AccountSearchModel searchModel);

}