﻿using System.ComponentModel.DataAnnotations;
using FrameWork.Application.FileUpload;
using FrameWork.Application.Messages;
using Microsoft.AspNetCore.Http;

namespace SM._Application.Contracts.ProductCategory.DTO_s;

public class CreateProductCategory
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Name { get; set; }

    public string Description { get; set; }

    [FileExtensionLimitation(new string[] { ".jpeg", ".jpg", ".png" }, ErrorMessage = ValidationMessages.InvalidFileFormat)]
    [MaxFileSize(3 * 1024 * 1024, ErrorMessage = ValidationMessages.MaxFileSize)]
    public IFormFile Picture { get; set; }
    public string PictureAlt { get; set; }
    public string PictureTitle { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Keywords { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string MetaDescription { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Slug { get; set; }

}