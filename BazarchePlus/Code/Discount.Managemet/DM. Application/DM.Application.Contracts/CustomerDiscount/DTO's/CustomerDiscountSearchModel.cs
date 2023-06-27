namespace DM.Application.Contracts.CustomerDiscount.DTO_s
{
    public class CustomerDiscountSearchModel
    {
        public long ProductId { get; set; }
        public long SellerId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
