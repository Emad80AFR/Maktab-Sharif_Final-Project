using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace AM._Application.Contracts.Account.DTO_s;

public class AccountSearchModel
{
    [Display(Name = "نام")]
    public string? Fullname { get; set; }

    [Display(Name = "نام کاربری")]
    public string? Username { get; set; }

    [Display(Name = "موبایل")]
    public string? Mobile { get; set; }

    [Display(Name = "نقش")]
    public long RoleId { get; set; }

    [Display(Name = "جستجو در غیرفعال ها")]
    public bool IsActive { get; set; }

}