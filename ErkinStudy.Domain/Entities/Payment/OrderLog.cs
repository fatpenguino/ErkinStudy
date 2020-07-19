using System;

namespace ErkinStudy.Domain.Entities.Payment
{
    public class OrderLog
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string Message { get; set; }
        public string ExtendedMessage { get; set; }
        public DateTime  CreatedTime { get; set; }
    }
}
