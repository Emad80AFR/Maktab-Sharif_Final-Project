using SM._Domain.OrderAgg;

namespace SM._Domain.Services;

public interface IShopAccountAcl
{
    Task<bool> UpdateFinancialInfo(OrderItem item,CancellationToken cancellationToken);
    Task<bool> CalculateSaleAmount(OrderItem item, CancellationToken cancellationToken);
}