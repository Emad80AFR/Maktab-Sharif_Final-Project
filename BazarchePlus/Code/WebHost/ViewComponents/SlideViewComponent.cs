using BP._Query.Contracts.Slide;
using Microsoft.AspNetCore.Mvc;

namespace WebHost.ViewComponents;

public class SlideViewComponent:ViewComponent
{
    private readonly ISlideQuery _query;

    public SlideViewComponent(ISlideQuery query)
    {
        _query = query;
    }

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
    {

        
        var slides = await _query.GetSlides(cancellationToken);
        return View(slides);
    }
}