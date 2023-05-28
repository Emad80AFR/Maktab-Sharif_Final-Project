using DM.Application.Contracts.CustomerDiscount.DTO_s;
using FrameWork.Application;

namespace DM.Application.Contracts.CustomerDiscount
{
    public interface ICustomerDiscountApplication
    {
        Task<OperationResult> Define(DefineCustomerDiscount command,CancellationToken cancellationToken);
        Task<OperationResult> Edit(EditCustomerDiscount command,CancellationToken cancellationToken);
        
        Task<EditCustomerDiscount> GetDetails(long id,CancellationToken cancellationToken);
        Task<List<CustomerDiscountViewModel>> Search(CustomerDiscountSearchModel searchModel, CancellationToken cancellationToken);
    }
}
