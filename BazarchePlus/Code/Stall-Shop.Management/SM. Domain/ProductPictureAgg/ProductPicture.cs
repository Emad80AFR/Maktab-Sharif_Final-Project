﻿using FrameWork.Domain;
using SM._Domain.ProductAgg;

namespace SM._Domain.ProductPictureAgg;

public class ProductPicture:BaseClass<long>
{
    public long ProductId { get; private set; }
    public string Picture { get; private set; }
    public string PictureAlt { get; private set; }
    public string PictureTitle { get; private set; }
    public bool IsRemoved { get; private set; }
    public Product Product { get; private set; }

    internal ProductPicture()
    {
        
    }
    public ProductPicture(long productId, string picture, string pictureAlt, string pictureTitle)
    {
        ProductId = productId;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        IsRemoved = false;
    }

    public void Edit(long productId, string picture, string pictureAlt, string pictureTitle)
    {
        ProductId = productId;

        if (!string.IsNullOrWhiteSpace(picture))
            Picture = picture;

        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
    }

    public void Remove()
    {
        IsRemoved = true;
    }

    public void Restore()
    {
        IsRemoved = false;
    }

}