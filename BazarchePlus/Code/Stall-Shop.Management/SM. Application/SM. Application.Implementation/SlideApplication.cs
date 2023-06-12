using FrameWork.Application;
using FrameWork.Application.FileUpload;
using FrameWork.Application.Messages;
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

    public async Task<OperationResult> Edit(EditSlide command, CancellationToken cancellationToken)
    {
        var operation = new OperationResult();
        var slide = await _slideRepository.Get(command.Id, cancellationToken);
        if (slide == null)
        {
            // Log a message at the warning level
            _logger.LogWarning("Slide not found for ID: {SlideId}", command.Id);
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        var pictureName = await _fileUploader.Upload(command.Picture, "slides", cancellationToken);

        slide.Edit(pictureName, command.PictureAlt, command.PictureTitle,
            command.Heading, command.Title, command.Text, command.Link, command.BtnText);

        await _slideRepository.SaveChanges(cancellationToken);

        // Log a message at the information level
        _logger.LogInformation("Slide edited successfully: ID {SlideId}", command.Id);

        return operation.Succeeded();
    }

    public async Task<OperationResult> Remove(long id, CancellationToken cancellationToken)
    {
        var operation = new OperationResult();
        var slide = await _slideRepository.Get(id, cancellationToken);
        if (slide == null)
        {
            // Log a message at the warning level
            _logger.LogWarning("Slide not found for ID: {SlideId}", id);
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        slide.Remove();

        await _slideRepository.SaveChanges(cancellationToken);

        // Log a message at the information level
        _logger.LogInformation("Slide removed successfully: ID {SlideId}", id);

        return operation.Succeeded();
    }

    public async Task<OperationResult> Restore(long id, CancellationToken cancellationToken)
    {
        var operation = new OperationResult();
        var slide = await _slideRepository.Get(id, cancellationToken);
        if (slide == null)
        {
            // Log a message at the warning level
            _logger.LogWarning("Slide not found for ID: {SlideId}", id);
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        slide.Restore();

        await _slideRepository.SaveChanges(cancellationToken);

        // Log a message at the information level
        _logger.LogInformation("Slide restore successfully: ID {SlideId}", id);

        return operation.Succeeded();
    }

    public async Task<EditSlide> GetDetails(long id, CancellationToken cancellationToken)
    {
        // Log a message at the information level
        _logger.LogInformation("Fetching slide details for ID: {SlideId}", id);

        var slide = await _slideRepository.GetDetails(id, cancellationToken);

        // Log a message at the information level
        _logger.LogInformation("Slide details retrieved successfully: ID {SlideId}", id);

        return slide;
    }

    public async Task<List<SlideViewModel>> GetList(CancellationToken cancellationToken)
    {
        // Log a message at the information level
        _logger.LogInformation("Fetching slide list");

        var slides = await _slideRepository.GetList(cancellationToken);

        // Log a message at the information level
        _logger.LogInformation("Slide list retrieved successfully");

        return slides;
    }
}