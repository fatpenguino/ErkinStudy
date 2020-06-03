using System.ComponentModel.DataAnnotations.Schema;

namespace ErkinStudy.Domain.Entities.Payment
{
    public class Payment
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long FolderId { get; set; }
        public long OrderId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string ExternalId { get; set; }
    }
}
