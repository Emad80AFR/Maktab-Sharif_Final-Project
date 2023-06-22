using SM._Application.Contracts.Order.DTO_s;

namespace SM._Application.Contracts.Order
{
    public interface IOrderApplication
    {
        Task<long> PlaceOrder(Cart cart,CancellationToken cancellationToken);
        Task<double> GetAmountBy(long id,CancellationToken cancellationToken);
        Task Cancel(long id,CancellationToken cancellationToken);
        Task<string> PaymentSucceeded(long orderId, long refId, CancellationToken cancellationToken);
        Task<List<OrderItemViewModel>> GetItems(long orderId,CancellationToken cancellationToken);
        Task<List<OrderViewModel>> Search(OrderSearchModel searchModel, CancellationToken cancellationToken);
    }
}