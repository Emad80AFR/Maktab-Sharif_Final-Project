namespace CM._Application.Contracts.Comment.DTO_s
{
    public class AddComment
    {
        public long Sender { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string Website { get; set; }
        public long OwnerRecordId { get; set; }
        public int Type { get; set; }
        public long ParentId { get; set; }
    }
}
