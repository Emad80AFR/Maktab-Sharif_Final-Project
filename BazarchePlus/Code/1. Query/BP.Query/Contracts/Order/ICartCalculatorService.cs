using SM._Application.Contracts.Order.DTO_s;

namespace BP._Query.Contracts.Order;

public interface ICartCalculatorService
{
        Task<Cart> ComputeCart (List<CartItem> cartItems,CancellationToken cancellationToken);
}