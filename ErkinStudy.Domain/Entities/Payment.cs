using System;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Domain.Enums;

namespace ErkinStudy.Domain.Entities
{
    public class Payment
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public long LessonId { get; set; }
        public DateTime CreationTime { get; set; }
        public int Amount { get; set; }
        public PaymentProvider Provider { get; set; }
        public PaymentStatus Status { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Lesson Lesson { get; set; }
    }
}