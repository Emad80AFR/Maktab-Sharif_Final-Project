using CM._Application.Contracts.Comment.DTO_s;
using FrameWork.Application;

namespace CM._Application.Contracts.Comment;

public interface ICommentApplication
{
    Task<OperationResult> Add(AddComment command,CancellationToken cancellationToken);
    Task<OperationResult> Confirm(long id,CancellationToken cancellationToken);
    Task<OperationResult> Cancel(long id,CancellationToken cancellationToken);
    Task<List<CommentViewModel>> Search(CommentSearchModel searchModel,CancellationToken cancellationToken);

}