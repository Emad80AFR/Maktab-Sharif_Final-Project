using AM._Application.Contracts.Account.DTO_s;
using FrameWork.Infrastructure.ConfigurationModel;
using SM._Application.Contracts.Order.DTO_s;

namespace WebHost.Services;

public class CalculateCartItemWage:ICalculateWage
{
    private readonly AppSettingsOption.Domainsettings _appOptions;
    public CalculateCartItemWage(AppSettingsOption.Domainsettings appOptions)
    {
        _appOptions = appOptions;
    }

    public void CalculateWageAsync(CartItem cartItem,FinancialModel sellerInfo,CancellationToken cancellationToken)
    {
        var item = cartItem;
        if (sellerInfo.Medal==_appOptions.Medals.Gold)
            cartItem.WageRate = _appOptions.Wage.GoldenWage;

        else if (sellerInfo.Medal == _appOptions.Medals.Silver)
            cartItem.WageRate = _appOptions.Wage.SilverWage;

        else if(sellerInfo.Medal == _appOptions.Medals.Bronze)
            cartItem.WageRate = _appOptions.Wage.BronzeWage;
        else
            cartItem.WageRate = _appOptions.Wage.Default;
        cartItem.SellerId = sellerInfo.Id;
        cartItem.Wage = (item.WageRate * item.UnitPrice)/100;
        cartItem.TotalItemWage = (item.WageRate * item.TotalItemPrice)/100;
        
    }
}