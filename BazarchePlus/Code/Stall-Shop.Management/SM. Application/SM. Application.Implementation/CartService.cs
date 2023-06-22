using SM._Application.Contracts.Order;
using SM._Application.Contracts.Order.DTO_s;

namespace SM._Application.Implementation
{
    public class CartService : ICartService
    {
        public Cart Cart { get; set; }

        public Cart Get()
        {
            return Cart;
        }

        public void Set(Cart cart)
        {
            Cart = cart;
        }
    }
}