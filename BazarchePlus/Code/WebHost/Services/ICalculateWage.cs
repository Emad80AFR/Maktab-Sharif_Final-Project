using AM._Application.Contracts.Account.DTO_s;
using SM._Application.Contracts.Order.DTO_s;

namespace WebHost.Services;

public interface ICalculateWage
{
    void CalculateWageAsync(CartItem cartItem,FinancialModel sellerInfo,CancellationToken cancellationToken);
}