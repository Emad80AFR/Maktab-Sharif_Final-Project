using FrameWork.Application;
using Microsoft.Extensions.Logging;
using SM._Application.Contracts.Slide;
using SM._Application.Contracts.Slide.DTO_s;
using SM._Domain.SlideAgg;

namespace SM._Application.Implementation;

public class SlideApplication:ISlideApplication
{
    private readonly ISlideRepository _slideRepository;
    private readonly ILogger<SlideApplication> _logger;
    private readonly IFileUploader _fileUploader;
    public SlideApplication(ILogger<SlideApplication> logger, ISlideRepository slideRepository, IFileUploader fileUploader)
    {
        _logger = logger;
        _slideRepository = slideRepository;
        _fileUploader = fileUploader;
    }


    public async Task<OperationResult> Create(CreateSlide command, CancellationToken cancellationToken)
    {
        var operation = new OperationResult();

        try
        {
            var pictureName = await _fileUploader.Upload(command.Picture, "slides", cancellationToken);

            var slide = new Slide(pictureName, command.PictureAlt, command.PictureTitle,
                command.Heading, command.Title, command.Text, command.Link, command.BtnText);

            await _slideRepository.Create(slide, cancellationToken);
            await _slideRepository.SaveChanges(cancellationToken);

            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            // Log the exception at the error level
            _logger.LogError(ex, "An error occurred while creating a slide.");
            return operation.Failed(ApplicationMessages.ErrorOccurred);
        }
        finally
        {
            // Log a message at the information level
            _logger.LogInformation("Slide creation operation completed.");
        }
    }

    public Task<OperationResult> Edit(EditSlide command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult> Remove(long id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult> Restore(long id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<EditSlide> GetDetails(long id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<SlideViewModel>> GetList(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}