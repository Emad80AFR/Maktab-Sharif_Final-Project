using System.ComponentModel.DataAnnotations;
using AM._Application.Contracts.Role.DTO_s;
using FrameWork.Application.Messages;
using Microsoft.AspNetCore.Http;

namespace AM._Application.Contracts.Account.DTO_s;

public class RegisterAccount
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Fullname { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Username { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Password { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Mobile { get; set; }

    public long RoleId { get; set; }

    public IFormFile ProfilePhoto { get; set; }
    public List<RoleViewModel> Roles { get; set; }

}