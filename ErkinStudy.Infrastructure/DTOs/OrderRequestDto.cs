namespace ErkinStudy.Infrastructure.DTOs
{
    public class OrderRequestDto
    {
        public long OrderId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
