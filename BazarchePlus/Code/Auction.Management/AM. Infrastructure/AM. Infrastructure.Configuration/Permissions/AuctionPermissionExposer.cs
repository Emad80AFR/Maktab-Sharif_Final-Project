using FrameWork.Infrastructure.Permission;

namespace AM._Infrastructure.Configuration.Permissions;

public class AuctionPermissionExposer:IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "Auctions", new List<PermissionDto>
                {
                    new(AuctionsPermissions.ListAuctions, "مشاهده لیست"),
                    new(AuctionsPermissions.SearchAuctions, "جستجو  "),
                    new(AuctionsPermissions.DefineAuction, "تعریف مزایده"),
                    new(AuctionsPermissions.EditAuction, "ویرایش "),
                    new(AuctionsPermissions.ActivateAuction, "فعال سازی "),
                    new(AuctionsPermissions.DeActiveAuction, "غیر فعال کردن "),

                }
            },
            {
                "AuctionMenu", new List<PermissionDto>
                {
                    new(AuctionsPermissions.Menu, "مشاهده منو")
                }
            }

        };
    }
}