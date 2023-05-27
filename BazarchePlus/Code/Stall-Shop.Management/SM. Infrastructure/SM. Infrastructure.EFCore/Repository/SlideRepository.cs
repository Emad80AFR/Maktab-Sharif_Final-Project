using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SM._Application.Contracts.Slide.DTO_s;
using SM._Domain.SlideAgg;

namespace SM._Infrastructure.EFCore.Repository;

public class SlideRepository:BaseRepository<long,Slide>,ISlideRepository
{
    private readonly ShopContext _shopContext;
    private readonly ILogger<SlideRepository> _logger;

    public SlideRepository(ShopContext shopContext, ILogger<SlideRepository> logger):base(shopContext,logger)
    {
        _shopContext = shopContext;
        _logger = logger;
    }

    public async Task<EditSlide> GetDetails(long id, CancellationToken cancellationToken)
    {
        // Log a message at the information level
        _logger.LogInformation("Fetching slide details for ID: {SlideId}", id);

        var slide = await _shopContext.Slides
            .Where(x => x.Id == id)
            .Select(x => new EditSlide
            {
                Id = x.Id,
                BtnText = x.BtnText,
                Heading = x.Heading,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Text = x.Text,
                Link = x.Link,
                Title = x.Title
            })
            .FirstOrDefaultAsync(cancellationToken);

        // Log a message at the information level if the slide details were found
        if (slide != null)
            _logger.LogInformation("Slide details retrieved successfully for ID: {SlideId}", id);
        else
            _logger.LogWarning("Slide details not found for ID: {SlideId}", id);

        return slide!;
    }

    public async Task<List<SlideViewModel>> GetList(CancellationToken cancellationToken)
    {
        // Log a message at the information level
        _logger.LogInformation("Fetching slide list...");

        var slides = await _shopContext.Slides
            .Select(x => new SlideViewModel
            {
                Id = x.Id,
                Heading = x.Heading,
                Picture = x.Picture,
                Title = x.Title,
                IsRemoved = x.IsRemoved,
                CreationDate = x.CreationDate.ToFarsi()
            })
            .OrderByDescending(x => x.Id)
            .ToListAsync(cancellationToken);

        // Log a message at the information level if the slide list was retrieved
        if (slides != null && slides.Any())
            _logger.LogInformation("Slide list retrieved successfully. Total slides: {SlideCount}", slides.Count);
        else
            _logger.LogWarning("Slide list is empty or could not be retrieved.");

        return slides;
    }
}