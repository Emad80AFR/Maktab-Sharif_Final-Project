using FrameWork.Domain;
using SM._Domain.ProductCategoryAgg;
using SM._Domain.ProductPictureAgg;

namespace SM._Domain.ProductAgg;

public class Product:BaseClass<long>
{
    public string Name { get; private set; }
    public string Code { get; private set; }
    public string ShortDescription { get; private set; }
    public string Description { get; private set; }
    public string Picture { get; private set; }
    public string PictureAlt { get; private set; }
    public string PictureTitle { get; private set; }
    public string Slug { get; private set; }
    public string Keywords { get; private set; }
    public string MetaDescription { get; private set; }
    public long CategoryId { get; private set; }
    public long SellerId { get; set; }
    public bool IsActive { get; set; }
    
    

    public ProductCategory Category { get; private set; }
    public List<ProductPicture> ProductPictures { get; private set; }

    internal Product()
    {
        
    }
    public Product(string name, string code, string shortDescription, string description,
        string picture, string pictureAlt, string pictureTitle, long categoryId, string slug,
        string keywords, string metaDescription, long sellerId)
    {
        Name = name;
        Code = code;
        ShortDescription = shortDescription;
        Description = description;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        CategoryId = categoryId;
        Slug = slug;
        Keywords = keywords;
        MetaDescription = metaDescription;
        SellerId = sellerId;
        IsActive = false;
    }

    public void Edit(string name, string code, string shortDescription, string description, string picture,
        string pictureAlt, string pictureTitle, long categoryId, string slug,
        string keywords, string metaDescription)
    {
        Name = name;
        Code = code;
        ShortDescription = shortDescription;
        Description = description;

        if (!string.IsNullOrWhiteSpace(picture))
            Picture = picture;

        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        CategoryId = categoryId;
        Slug = slug;
        Keywords = keywords;
        MetaDescription = metaDescription;
    }

    public void Activate()
    {
        IsActive = true;
    }
    public void DeActive()
    {
        IsActive = false;
    }
}