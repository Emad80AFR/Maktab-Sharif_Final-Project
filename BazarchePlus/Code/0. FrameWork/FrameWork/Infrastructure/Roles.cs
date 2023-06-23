namespace FrameWork.Infrastructure
{
    public static class Roles
    {
        public const string Developer = "1";
        public const string Administrator = "2";
        public const string Seller = "3";
        public const string Customer = "4";
        public const string ContentUploader = "5";

        public static string GetRoleBy(long id)
        {
            return id switch
            {
                1 => "توسعه دهنده",
                2 => "مدیرسیستم",
                3 => "فروشنده",
                _ => ""
            };
        }
    }
}
