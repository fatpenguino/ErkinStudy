using System;

namespace ErkinStudy.Domain.Entities.Payment
{
    public class OrderOperation
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string Message { get; set; }
        public DateTime TraceTime { get; set; }
        public virtual Order Order { get; set; }
    }
}
