using BP._Query.Contracts.ArticleCategory;
using BP._Query.Contracts.ProductCategory;

namespace BP._Query
{
    public class MenuModel
    {
        public List<ProductCategoryQueryModel> ProductCategories { get; set; }
        public List<ArticleCategoryQueryModel> ArticleCategories { get; set; }

    }
}
