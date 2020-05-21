using System;

namespace ErkinStudy.Domain.Entities.Payment
{
    public class Order
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long FolderId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}
