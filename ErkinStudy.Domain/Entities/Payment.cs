using System;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Domain.Enums;

namespace ErkinStudy.Domain.Entities
{
    public class Payment
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ApproverId { get; set; }
        public long ProductId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ApprovedTime { get; set; }
        public long Amount { get; set; }
        public PaymentProvider Provider { get; set; }
        public PaymentStatus Status { get; set; }
        public ProductType ProductType { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser Approver { get; set; }
    }
}