namespace ErkinStudy.Domain.Entities.Payment
{
    public class Payment
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long FolderId { get; set; }
        public long OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
