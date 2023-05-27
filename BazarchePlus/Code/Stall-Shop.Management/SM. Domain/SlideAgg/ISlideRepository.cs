using FrameWork.Domain;
using SM._Application.Contracts.Slide.DTO_s;

namespace SM._Domain.SlideAgg;

public interface ISlideRepository:IBaseRepository<long,Slide>
{
   Task<EditSlide>  GetDetails(long id, CancellationToken cancellationToken);
    Task<List<SlideViewModel>> GetList(CancellationToken cancellationToken);

}