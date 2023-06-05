using CM._Application.Contracts.Comment.DTO_s;
using FrameWork.Domain;

namespace CM._Domain.CommentAgg;

public interface ICommentRepository:IBaseRepository<long,Comment>
{
    Task<List<CommentViewModel>> Search(CommentSearchModel searchModel, CancellationToken cancellationToken);

}