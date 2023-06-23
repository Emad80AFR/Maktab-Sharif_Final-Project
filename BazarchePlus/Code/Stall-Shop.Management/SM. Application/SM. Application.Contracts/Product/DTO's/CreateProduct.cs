using FrameWork.Application.Messages;
using Microsoft.AspNetCore.Http;
using SM._Application.Contracts.ProductCategory.DTO_s;
using System.ComponentModel.DataAnnotations;

namespace SM._Application.Contracts.Product.DTO_s;

public class CreateProduct
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Name { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Code { get; set; }

    public long SellerId { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string ShortDescription { get; set; }

    public string Description { get; set; }
    public IFormFile Picture { get; set; }
    public string PictureAlt { get; set; }
    public string PictureTitle { get; set; }

    [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
    public long CategoryId { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Slug { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Keywords { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string MetaDescription { get; set; }
    public List<ProductCategoryViewModel> Categories { get; set; }

}