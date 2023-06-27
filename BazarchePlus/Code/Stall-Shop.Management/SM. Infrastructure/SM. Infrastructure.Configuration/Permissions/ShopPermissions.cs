namespace SM._Infrastructure.Configuration.Permissions
{
    public static class ShopPermissions
    {
        //Product
        public const int ListProducts = 10;
        public const int SearchProducts = 11;
        public const int CreateProduct = 12;
        public const int EditProduct = 13;
        public const int ActivateProduct = 14;
        public const int DeActivateProduct
            = 15;

        //ProductCategory
        public const int ListProductCategories = 20;
        public const int SearchProductCategories = 21;
        public const int CreateProductCategory = 22;
        public const int EditProductCategory = 23;


        //ProductPicture
        public const int ListProductPictures = 30;
        public const int SearchProductPictures = 31;
        public const int CreateProductPicture = 32;
        public const int EditProductPicture = 33;
        public const int DeleteProductPicture = 34;
        public const int RestoreProductPicture = 35;


        //Slide
        public const int ListSlide = 40;
        public const int CreateSlide = 41;
        public const int EditSlide = 42;
        public const int DeleteSlide = 43;
        public const int RestoreSlide = 44;

        //Menu
        public const int Menu = 50;
        public const int ProductCategoryMenu = 51;
        public const int SlideMenu = 52;

        //Order
        public const int OrderMenu = 60;
        public const int SearchOrder = 61;
        public const int SearchByAccount = 62;
        public const int RejectOrder =63;
        public const int ConfirmOrder =64;
        public const int SeeOrder =65;
        public const int SeeOrderItems =66;


    }
}