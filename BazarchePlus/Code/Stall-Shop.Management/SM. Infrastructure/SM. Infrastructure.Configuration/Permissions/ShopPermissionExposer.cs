using FrameWork.Infrastructure.Permission;

namespace SM._Infrastructure.Configuration.Permissions
{
    public class ShopPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string, List<PermissionDto>>
            {
                {
                    "Product", new List<PermissionDto>
                    {
                        new(ShopPermissions.ListProducts, "مشاهده لیست"),
                        new(ShopPermissions.SearchProducts, "جستجو"),
                        new(ShopPermissions.CreateProduct, "ایجاد"),
                        new(ShopPermissions.EditProduct, "ویرایش"),
                        new(ShopPermissions.ActivateProduct, "فعال سازی"),
                        new(ShopPermissions.DeActivateProduct, "غیر فعال سازی")
                    }
                },
                {
                    "ProductCategory", new List<PermissionDto>
                    {
                        new(ShopPermissions.ListProductCategories, "مشاهده لیست"),
                        new(ShopPermissions.SearchProductCategories, "جستجو"),
                        new(ShopPermissions.CreateProductCategory, "ایجاد"),
                        new(ShopPermissions.EditProductCategory, "ویرایش"),
                    }
                },
                {
                    "ProductPicture", new List<PermissionDto>
                    {
                        new(ShopPermissions.ListProductPictures, "مشاهده لیست"),
                        new(ShopPermissions.SearchProductPictures, "جستجو"),
                        new(ShopPermissions.CreateProductPicture, "ایجاد"),
                        new(ShopPermissions.EditProductPicture, "ویرایش"),
                        new(ShopPermissions.DeleteProductPicture, "حذف"),
                        new(ShopPermissions.RestoreProductPicture, "فعال سازی"),
                    }
                },
                {
                    "Slide", new List<PermissionDto>
                    {
                        new(ShopPermissions.ListSlide, "مشاهده لیست"),
                        new(ShopPermissions.CreateSlide, "ایجاد"),
                        new(ShopPermissions.EditSlide, "ویرایش"),
                        new(ShopPermissions.DeleteSlide, "حذف"),
                        new(ShopPermissions.RestoreSlide, "فعال سازی"),
                    }
                }
                ,
                {
                    "ShopMenu", new List<PermissionDto>
                    {
                        new(ShopPermissions.Menu, "مشاهده منو"),
                        new(ShopPermissions.ProductCategoryMenu, "زیر منو گروه محصولی"),
                        new(ShopPermissions.SlideMenu, "زیرمنو اسلاید"),
                    }
                }
            };
        }
    }
}