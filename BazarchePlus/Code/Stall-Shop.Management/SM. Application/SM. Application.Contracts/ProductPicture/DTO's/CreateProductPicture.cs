using FrameWork.Application.Messages;
using Microsoft.AspNetCore.Http;
using SM._Application.Contracts.Product.DTO_s;
using System.ComponentModel.DataAnnotations;
using FrameWork.Application.FileOpload;

namespace SM._Application.Contracts.ProductPicture.DTO_s;

public class CreateProductPicture
{
    [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
    public long ProductId { get; set; }

    [MaxFileSize(1 * 1024 * 1024, ErrorMessage = ValidationMessages.MaxFileSize)]
    public IFormFile Picture { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string PictureAlt { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string PictureTitle { get; set; }
    public List<ProductViewModel> Products { get; set; }

}