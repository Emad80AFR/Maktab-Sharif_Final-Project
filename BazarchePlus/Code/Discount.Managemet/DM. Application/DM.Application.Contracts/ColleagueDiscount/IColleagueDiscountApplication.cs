using DM.Application.Contracts.ColleagueDiscount.DTO_s;
using FrameWork.Application;

namespace DM.Application.Contracts.ColleagueDiscount;

public interface IColleagueDiscountApplication
{
    Task<OperationResult> Define(DefineColleagueDiscount command,CancellationToken cancellationToken);
    Task<OperationResult> Edit(EditColleagueDiscount command,CancellationToken cancellationToken);
    Task<OperationResult> Remove(long id,CancellationToken cancellationToken);
    Task<OperationResult> Restore(long id,CancellationToken cancellationToken);
    Task<EditColleagueDiscount> GetDetails(long id,CancellationToken cancellationToken);
    Task<List<ColleagueDiscountViewModel>> Search(ColleagueDiscountSearchModel searchModel,CancellationToken cancellationToken);
}