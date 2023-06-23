namespace FrameWork.Application.Messages;

public class ApplicationMessages
{
    public const string DuplicatedRecord = "امکان ثبت رکورد تکراری وجود ندارد. لطفا مجدد تلاش بفرمایید.";
    public const string RecordNotFound = "رکورد با اطلاعات درخواست شده یافت نشد. لطفا مجدد تلاش بفرمایید.";
    public static string PasswordsNotMatch = "پسورد و تکرار آن با هم مطابقت ندارند";
    public static string WrongUserPass = "نام کاربری یا کلمه رمز اشتباه است";
    public static string ErrorOccurred { get; set; } = "خطایی رخ داده،لطفا با مدیر سیستم تماس بگیرید.";
    public static string UnActiveAccount { get; set; } = "اکانت شما هنوز تایید نشده،بعد از چند دقیقه مجددا امتحان کنید.";
}