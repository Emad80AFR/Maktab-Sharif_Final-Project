using FrameWork.Infrastructure.Permission;

namespace BM._Infrastructure.Configuration.Permissions;

public class BlogPermissionExposer:IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "ArticleCategory", new List<PermissionDto>
                {
                    new(BlogPermissions.ListArticleCategory, "مشاهده لیست"),
                    new(BlogPermissions.SearchArticleCategory, "جستجو  "),
                    new(BlogPermissions.CreateArticleCategory, "ایجاد"),
                    new(BlogPermissions.EditArticleCategory, "ویرایش ")
                }
            },
            {
                "Article", new List<PermissionDto>
                {
                    new(BlogPermissions.ListArticle, "مشاهده لیست"),
                    new(BlogPermissions.SearchArticle, "جستجو  "),
                    new(BlogPermissions.CreateArticle, "ایجاد"),
                    new(BlogPermissions.EditArticle, "ویرایش ")
                }
            },
            {
                "BlogMenu", new List<PermissionDto>
                {
                    new(BlogPermissions.Menu, "مشاهده منو")
                }
            }

        };
    }
}