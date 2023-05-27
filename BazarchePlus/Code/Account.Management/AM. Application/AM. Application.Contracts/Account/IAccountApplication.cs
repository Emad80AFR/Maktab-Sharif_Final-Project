﻿using AM._Application.Contracts.Account.DTO_s;
using FrameWork.Application;

namespace AM._Application.Contracts.Account;

public interface IAccountApplication
{
    Task<AccountViewModel> GetAccountBy(long id, CancellationToken cancellationToken);
    Task<OperationResult> Register(RegisterAccount command, CancellationToken cancellationToken);
    Task<OperationResult> Edit(EditAccount command, CancellationToken cancellationToken);
    Task<OperationResult> ChangePassword(ChangePassword command, CancellationToken cancellationToken);
    Task<OperationResult> Login(Login command, CancellationToken cancellationToken);
    Task<EditAccount> GetDetails(long id, CancellationToken cancellationToken);
    Task<List<AccountViewModel>> Search(AccountSearchModel searchModel, CancellationToken cancellationToken);
    Task Logout(CancellationToken cancellationToken);
    Task<List<AccountViewModel>> GetAccounts(CancellationToken cancellationToken);

}