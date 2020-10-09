namespace ErkinStudy.Infrastructure.DTOs.Payment
{
    public class OrderResponseDto
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string OperationUrl { get; set; }
        public int OperationId { get; set; }
    }
}
