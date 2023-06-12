namespace FrameWork.Infrastructure
{
    public static class Roles
    {
        public const string Administrator = "1";
        public const string Customer = "3";
        public const string Seller = "4";
        
        public static string GetRoleBy(long id)
        {
            return id switch
            {
                1 => "مدیرسیستم",
                4 => "فروشنده",
                _ => ""
            };
        }
    }
}
