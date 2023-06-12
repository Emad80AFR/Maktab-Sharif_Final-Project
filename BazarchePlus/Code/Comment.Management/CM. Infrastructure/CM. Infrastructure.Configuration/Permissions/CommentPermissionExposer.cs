using FrameWork.Infrastructure.Permission;

namespace CM._Infrastructure.Configuration.Permissions;

public class CommentPermissionExposer:IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "Comments", new List<PermissionDto>
                {
                    new(CommentsPermissions.ListComments, "مشاهده لیست"),
                    new(CommentsPermissions.SearchComments, "جستجو  "),
                    new(CommentsPermissions.CancelComment, "رد کردن پیام"),
                    new(CommentsPermissions.ConfirmComment, "تایید پیام ")

                }
            }

        };
    }
}