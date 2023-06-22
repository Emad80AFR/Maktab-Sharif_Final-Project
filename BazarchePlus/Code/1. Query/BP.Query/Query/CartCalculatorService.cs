using BP._Query.Contracts.Order;
using DM._Infrastructure.EFCore;
using FrameWork.Application.Authentication;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SM._Application.Contracts.Order.DTO_s;

namespace BP._Query.Query;

public class CartCalculatorService:ICartCalculatorService
{
    private readonly IAuthHelper _authHelper;
    private readonly DiscountContext _discountContext;

    public CartCalculatorService(IAuthHelper authHelper, DiscountContext discountContext)
    {
        _authHelper = authHelper;
        _discountContext = discountContext;
    }

    public async Task<Cart> ComputeCart(List<CartItem> cartItems, CancellationToken cancellationToken)
    {
        var cart = new Cart();
        var colleagueDiscounts = await _discountContext.ColleagueDiscounts
            .Where(x => !x.IsRemoved)
            .Select(x => new { x.DiscountRate, x.ProductId })
            .ToListAsync(cancellationToken: cancellationToken);

        var customerDiscounts = await _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId })
            .ToListAsync(cancellationToken: cancellationToken);
        var currentAccountRole = _authHelper.CurrentAccountRole();

        foreach (var cartItem in cartItems)
        {
            if (currentAccountRole == Roles.Seller)
            {
                var colleagueDiscount = colleagueDiscounts.FirstOrDefault(x => x.ProductId == cartItem.Id);
                if (colleagueDiscount != null)
                    cartItem.DiscountRate = colleagueDiscount.DiscountRate;
            }
            else
            {
                var customerDiscount = customerDiscounts.FirstOrDefault(x => x.ProductId == cartItem.Id);
                if (customerDiscount != null)
                    cartItem.DiscountRate = customerDiscount.DiscountRate;
            }

            cartItem.DiscountAmount = ((cartItem.TotalItemPrice * cartItem.DiscountRate) / 100);
            cartItem.ItemPayAmount = cartItem.TotalItemPrice - cartItem.DiscountAmount;
            cart.Add(cartItem);
        }

        return cart;
    }
}