using CM._Application.Contracts.Comment;
using CM._Application.Contracts.Comment.DTO_s;
using CM._Domain.CommentAgg;
using FrameWork.Application;
using FrameWork.Application.Messages;
using Microsoft.Extensions.Logging;

namespace CM._Application.Implementation;

public class CommentApplication:ICommentApplication
{
    private readonly ILogger<CommentApplication> _logger;
    private readonly ICommentRepository _commentRepository;

    public CommentApplication(ILogger<CommentApplication> logger, ICommentRepository commentRepository)
    {
        _logger = logger;
        _commentRepository = commentRepository;
    }

    public async Task<OperationResult> Add(AddComment command, CancellationToken cancellationToken)
    {
        try
        {
            var operation = new OperationResult();
            var comment = new Comment(command.Name, command.Email, command.Website, command.Message,
                command.OwnerRecordId, command.Type, command.ParentId);

            await _commentRepository.Create(comment,cancellationToken);
            await _commentRepository.SaveChanges(cancellationToken);

            _logger.LogInformation("Comment added successfully.");

            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a comment.");

            throw; 
        }
    }

    public async Task<OperationResult> Confirm(long id, CancellationToken cancellationToken)
    {
        try
        {
            var operation = new OperationResult();
            var comment = await _commentRepository.Get(id,cancellationToken);
            if (comment == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            comment.Confirm();
            await _commentRepository.SaveChanges(cancellationToken);

            _logger.LogInformation("Comment confirmed successfully.");

            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while confirming a comment.");

            throw; 
        }
    }

    public async Task<OperationResult> Cancel(long id, CancellationToken cancellationToken)
    {
        try
        {
            var operation = new OperationResult();
            var comment = await _commentRepository.Get(id,cancellationToken);
            if (comment == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            comment.Cancel();
            await _commentRepository.SaveChanges(cancellationToken);

            _logger.LogInformation("Comment cancelled successfully.");

            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while cancelling a comment.");

            throw; 
        }
    }

    public async Task<List<CommentViewModel>> Search(CommentSearchModel searchModel, CancellationToken cancellationToken)
    {
        try
        {
            var results = await _commentRepository.Search(searchModel, cancellationToken);

            _logger.LogInformation("Comment search completed successfully.");

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while searching for comments.");

            throw; 
        }
    }
}