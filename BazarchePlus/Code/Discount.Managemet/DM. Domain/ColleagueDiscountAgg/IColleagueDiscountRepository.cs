using DM.Application.Contracts.ColleagueDiscount.DTO_s;
using FrameWork.Domain;

namespace DM._Domain.ColleagueDiscountAgg
{
    public interface IColleagueDiscountRepository : IBaseRepository<long, ColleagueDiscount>
    {
        Task<EditColleagueDiscount> GetDetails(long id, CancellationToken cancellationToken);
        Task<List<ColleagueDiscountViewModel>> Search(ColleagueDiscountSearchModel searchModel, CancellationToken cancellationToken);
    }
}
