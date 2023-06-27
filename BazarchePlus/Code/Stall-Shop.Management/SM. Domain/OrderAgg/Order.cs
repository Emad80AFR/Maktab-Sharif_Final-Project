using FrameWork.Domain;

namespace SM._Domain.OrderAgg
{
    public class Order : BaseClass<long>
    {
        public long AccountId { get; private set; }
        public int PaymentMethod { get; private set; }
        public double TotalAmount { get; private set; }
        public double DiscountAmount { get; private set; }
        public double PayAmount { get; private set; }
        public double WageAmount { get; set; }
        public bool IsPaid { get; private set; }
        public bool IsCanceled { get; private set; }
        public string? IssueTrackingNo { get; private set; }
        public long RefId { get; private set; }
        public List<OrderItem> Items { get; private set; }

        public Order(long accountId, int paymentMethod, double totalAmount, double discountAmount, double payAmount, double wageAmount)
        {
            AccountId = accountId;
            TotalAmount = totalAmount;
            DiscountAmount = discountAmount;
            PayAmount = payAmount;
            PaymentMethod = paymentMethod;
            IsPaid = false;
            IsCanceled = false;
            RefId = 0;
            Items = new List<OrderItem>();
            WageAmount = wageAmount;
        }

        public void PaymentSucceeded(long refId)
        {
            IsPaid = true;

            if (refId != 0)
                RefId = refId;
        }

        public void Cancel()
        {
            IsCanceled = true;
        }

        public void SetIssueTrackingNo(string number)
        {
            IssueTrackingNo = number;
        }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
        }
    }
}