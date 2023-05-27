using BP._Query.Contracts.Slide;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SM._Infrastructure.EFCore;

namespace BP._Query.Query
{
    public class SlideQuery : ISlideQuery
    {
        private readonly ShopContext _shopContext;
        private readonly ILogger<SlideQuery> _logger;
        public SlideQuery(ShopContext shopContext, ILogger<SlideQuery> logger)
        {
            _shopContext = shopContext;
            _logger = logger;
        }

        public async Task<List<SlideQueryModel>> GetSlides(CancellationToken cancellationToken)
        {
            try
            {
                // Log a message at the information level
                _logger.LogInformation("Fetching slides...");

                var slides = await _shopContext.Slides
                    .Where(x => !x.IsRemoved)
                    .Select(x => new SlideQueryModel
                    {
                        Picture = x.Picture,
                        PictureAlt = x.PictureAlt,
                        PictureTitle = x.PictureTitle,
                        BtnText = x.BtnText,
                        Heading = x.Heading,
                        Link = x.Link,
                        Text = x.Text,
                        Title = x.Title
                    })
                    .ToListAsync(cancellationToken);

                // Log a message at the information level if the slides were retrieved
                if (slides != null && slides.Any())
                    _logger.LogInformation("Slides retrieved successfully. Total slides: {SlideCount}", slides.Count);
                else
                    _logger.LogWarning("No slides found.");

                return slides!;
            }
            catch (Exception ex)
            {
                // Log the error message
                _logger.LogError(ex, "An error occurred while fetching slides.");
                throw; // Rethrow the exception to propagate it to the caller
            }
        }
    }
}
