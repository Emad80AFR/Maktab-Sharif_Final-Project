using Microsoft.AspNetCore.Http;

namespace AM._Application.Contracts.Account.DTO_s;

public class EditAccount : RegisterAccount
{
    public long Id { get; set; }
    public string ProfilePictureName { get; set; }
    public IFormFile ShopPicture { get; set; }
    public string? ShopPictureName { get; set; }
    public string? ShopName { get; set; }

}