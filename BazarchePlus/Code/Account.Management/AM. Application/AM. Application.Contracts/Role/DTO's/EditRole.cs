using FrameWork.Infrastructure;

namespace AM._Application.Contracts.Role.DTO_s;

public class EditRole : CreateRole
{
    public long Id { get; set; }
    public List<PermissionDto> MappedPermissions { get; set; }

    public EditRole()
    {
        Permissions = new List<int>();
    }

}