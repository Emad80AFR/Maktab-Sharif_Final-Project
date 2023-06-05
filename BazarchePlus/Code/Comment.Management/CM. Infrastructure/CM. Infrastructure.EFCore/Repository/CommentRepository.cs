using CM._Application.Contracts.Comment.DTO_s;
using CM._Domain.CommentAgg;
using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CM._Infrastructure.EFCore.Repository;

public class CommentRepository:BaseRepository<long,Comment>,ICommentRepository
{
    private readonly ILogger<CommentRepository> _logger;
    private readonly CommentContext _commentContext;

    public CommentRepository(CommentContext commentContext, ILogger<CommentRepository> logger):base(commentContext,logger)
    {
        _commentContext = commentContext;
        _logger = logger;
    }

    public async Task<List<CommentViewModel>> Search(CommentSearchModel searchModel, CancellationToken cancellationToken)
    {
        try
        {
            var query = _commentContext.Comments
                .Select(x => new CommentViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Website = x.Website,
                    Message = x.Message,
                    OwnerRecordId = x.OwnerRecordId,
                    Type = x.Type,
                    IsCanceled = x.IsCanceled,
                    IsConfirmed = x.IsConfirmed,
                    CommentDate = x.CreationDate.ToFarsi()
                });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            if (!string.IsNullOrWhiteSpace(searchModel.Email))
                query = query.Where(x => x.Email.Contains(searchModel.Email));

            var results = await query.OrderByDescending(x => x.Id).ToListAsync(cancellationToken);

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