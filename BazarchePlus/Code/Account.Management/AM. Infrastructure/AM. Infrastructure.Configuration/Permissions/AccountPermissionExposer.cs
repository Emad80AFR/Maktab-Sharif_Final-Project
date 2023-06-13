using FrameWork.Infrastructure.Permission;

namespace AM._Infrastructure.Configuration.Permissions;

public class AccountPermissionExposer:IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "AccountMenu", new List<PermissionDto>
                {
                    new(AccountsPermissions.Menu, "مشاهده منو")
                }
            }

        };
    }
}