using FrameWork.Application;
using SM._Application.Contracts.Slide.DTO_s;

namespace SM._Application.Contracts.Slide
{
    public interface ISlideApplication
    {
        Task<OperationResult>  Create(CreateSlide command, CancellationToken cancellationToken);
        Task<OperationResult> Edit(EditSlide command,CancellationToken cancellationToken);
        Task<OperationResult> Remove(long id, CancellationToken cancellationToken);
        Task<OperationResult> Restore(long id, CancellationToken cancellationToken);
        Task<EditSlide> GetDetails(long id, CancellationToken cancellationToken);
        Task<List<SlideViewModel>> GetList(CancellationToken cancellationToken);
    }
}
