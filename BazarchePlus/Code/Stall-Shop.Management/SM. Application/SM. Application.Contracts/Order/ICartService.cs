using SM._Application.Contracts.Order.DTO_s;

namespace SM._Application.Contracts.Order
{
    public interface ICartService
    {
        Cart Get();
        void Set(Cart cart);

    }
}