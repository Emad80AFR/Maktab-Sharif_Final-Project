using AM._Application.Contracts.Account;
using SM._Domain.OrderAgg;
using SM._Domain.Services;

namespace SM._Infrastructure.AccountAcl
{
    public class ShopAccountAc:IShopAccountAcl
    {
        private readonly IAccountApplication _accountApplication;

        public ShopAccountAc(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }

        public async Task<bool> UpdateFinancialInfo(OrderItem item, CancellationToken cancellationToken)
        {
            var priceWithDiscount = item.UnitPrice - item.UnitPrice * item.DiscountRate / 100;
            var wage = priceWithDiscount * item.WageRate / 100;
            var sellerSaleAmount = priceWithDiscount - wage;
            return await _accountApplication.UpdateFinancialInfo(item.SellerId, sellerSaleAmount, cancellationToken);
            //if(!await _accountApplication.UpdateSaleAmount(wage, cancellationToken))
            //    return false;
        }

        public async Task<bool> CalculateSaleAmount(OrderItem item, CancellationToken cancellationToken)
        {
            var priceWithDiscount = item.UnitPrice - item.UnitPrice * item.DiscountRate / 100;
            var wage = priceWithDiscount * item.WageRate / 100;
            return await _accountApplication.UpdateSaleAmount(wage, cancellationToken);
        }

        //public async Task<bool> AssignMedal(long sellerId, CancellationToken cancellationToken)
        //{
        //   return await _accountApplication.AssignModel(sellerId,cancellationToken);
        //}
    }
}