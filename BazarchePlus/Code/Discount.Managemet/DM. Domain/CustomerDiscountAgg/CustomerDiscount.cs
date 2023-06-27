using System.ComponentModel;
using FrameWork.Domain;

namespace DM._Domain.CustomerDiscountAgg
{
    public class CustomerDiscount : BaseClass<long>
    {
        public long ProductId { get; private set; }
        public long SellerId { get; set; }
        public int DiscountRate { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Reason { get; private set; }

        public CustomerDiscount(long productId, int discountRate, DateTime startDate, 
            DateTime endDate, string reason, long sellerId)
        {
            ProductId = productId;
            DiscountRate = discountRate;
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
            SellerId = sellerId;
        }

        public void Edit(long productId, int discountRate, DateTime startDate,
            DateTime endDate, string reason)
        {
            ProductId = productId;
            DiscountRate = discountRate;
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
        }
    }
}
