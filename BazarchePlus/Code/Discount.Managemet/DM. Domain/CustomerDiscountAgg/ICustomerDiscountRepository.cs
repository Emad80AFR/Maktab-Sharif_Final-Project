using DM.Application.Contracts.CustomerDiscount.DTO_s;
using FrameWork.Domain;

namespace DM._Domain.CustomerDiscountAgg
{
    public interface ICustomerDiscountRepository : IBaseRepository<long, CustomerDiscount>
    {
        Task<EditCustomerDiscount> GetDetails(long id, CancellationToken cancellationToken);
        Task<List<CustomerDiscountViewModel>> Search(CustomerDiscountSearchModel searchModel, CancellationToken cancellationToken);
    }
}
