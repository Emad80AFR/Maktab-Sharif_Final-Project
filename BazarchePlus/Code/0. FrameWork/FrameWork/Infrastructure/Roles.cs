namespace FrameWork.Infrastructure
{
    public static class Roles
    {
        public const string Administrator = "1";
        public const string Customer = "3";
        public const string Seller = "4";
        
        public static string GetRoleBy(long id)
        {
            switch (id)
            {
                case 1:
                    return "مدیرسیستم";
                case 4:
                    return "فروشنده";
                default:
                    return "";
            }
        }
    }
}
