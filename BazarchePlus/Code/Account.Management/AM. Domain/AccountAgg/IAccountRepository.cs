using AM._Application.Contracts.Account.DTO_s;
using FrameWork.Domain;

namespace AM._Domain.AccountAgg;

public interface IAccountRepository:IBaseRepository<long,Account>
{
    Task<Account> GetBy(string username, CancellationToken cancellationToken);
    Task<EditAccount> GetDetails(long id, CancellationToken cancellationToken);
    Task<List<AccountViewModel>> GetAccounts(CancellationToken cancellationToken);
    Task<List<AccountViewModel>> Search(AccountSearchModel searchModel, CancellationToken cancellationToken);
    Task<FinancialModel> GetFinancialInfo(long id, CancellationToken cancellationToken);
    Task<Account> GetManagerAccount(CancellationToken cancellationToken);
    Task<string> GetAccountName(long id, CancellationToken cancellationToken);

}