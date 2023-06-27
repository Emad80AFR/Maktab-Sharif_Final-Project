using System.Reflection.Metadata.Ecma335;
using FrameWork.Domain;

namespace SM._Domain.OrderAgg
{
    public class OrderItem : BaseClass<long>
    {
        public long ProductId { get; private set; }
        public int Count { get; private set; }
        public double UnitPrice { get; private set; }
        public int DiscountRate { get; private set; }
        public int WageRate { get; set; }
        public long OrderId { get; private set; }
        public long SellerId { get; set; }
        public Order Order { get; private set; }

        public OrderItem(long productId, int count, double unitPrice, int discountRate, int wageRate,long sellerId)
        {
            ProductId = productId;
            Count = count;
            UnitPrice = unitPrice;
            DiscountRate = discountRate;
            WageRate = wageRate;
            SellerId = sellerId;
        }
    }
}