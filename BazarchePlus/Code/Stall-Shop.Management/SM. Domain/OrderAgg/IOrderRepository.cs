using FrameWork.Domain;
using SM._Application.Contracts.Order.DTO_s;

namespace SM._Domain.OrderAgg
{
    public interface IOrderRepository : IBaseRepository<long, Order>
    {
        Task<double> GetAmountBy(long id,CancellationToken cancellationToken);
        Task<List<OrderItemViewModel>> GetItems(long orderId, CancellationToken cancellationToken);
        Task<List<OrderViewModel>> Search(OrderSearchModel searchModel, CancellationToken cancellationToken);
        Task<bool> CheckPlaceOrder(long id,long productId, CancellationToken cancellationToken);
    }
}