using BP._Query;
using BP._Query.Contracts.ArticleCategory;
using BP._Query.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace WebHost.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IArticleCategoryQuery _articleCategoryQuery;
        public MenuViewComponent(IProductCategoryQuery productCategoryQuery, IArticleCategoryQuery articleCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
            _articleCategoryQuery = articleCategoryQuery;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            var result = new MenuModel
            {
                ProductCategories = await _productCategoryQuery.GetProductCategories(cancellationToken),
                ArticleCategories = await _articleCategoryQuery.GetArticleCategories(cancellationToken)
                
            };
            return View(result);
        }
    }
}
