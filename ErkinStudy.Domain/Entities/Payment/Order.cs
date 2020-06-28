using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Domain.Entities.Lessons;
using ErkinStudy.Domain.Enums;

namespace ErkinStudy.Domain.Entities.Payment
{
    public class Order
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long FolderId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string ExternalId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Folder Folder { get; set; }
        public virtual ICollection<OrderOperation> OrderOperations { get; set; }
    }
}
