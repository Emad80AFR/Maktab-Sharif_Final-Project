using FrameWork.Infrastructure.Permission;

namespace DM._Infrastructure.Configuration.Permissions;

public class DiscountPermissionExposer:IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "CustomerDiscounts", new List<PermissionDto>
                {
                    new(DiscountsPermissions.ListCustomerDiscounts, "مشاهده لیست"),
                    new(DiscountsPermissions.SearchCustomerDiscounts, "جستجو  "),
                    new(DiscountsPermissions.DefineCustomerDiscounts, "تعریف تخفیف"),
                    new(DiscountsPermissions.EditCustomerDiscount, "ویرایش ")
                    
                }
            },
            {
                "ColleagueDiscounts", new List<PermissionDto>
                {
                    new(DiscountsPermissions.ListColleagueDiscounts, "مشاهده لیست"),
                    new(DiscountsPermissions.SearchColleagueDiscounts, "جستجو  "),
                    new(DiscountsPermissions.DefineColleagueDiscounts, "تعریف تخفیف"),
                    new(DiscountsPermissions.EditColleagueDiscount, "ویرایش "),
                    new(DiscountsPermissions.DeActiveColleagueDiscount, "فعال سازی "),
                    new(DiscountsPermissions.ActiveColleagueDiscount, "غیر فعال کردن "),

                }
            },
            {
                "DiscountMenu", new List<PermissionDto>
                {
                    new(DiscountsPermissions.Menu, "مشاهده منو")
                }
            }

        };
    }
}