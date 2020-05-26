namespace ErkinStudy.Web.Models
{
    public class NewOrderViewModel
    {
        public long UserId { get; set; }
        public long FolderId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public long Amount { get; set; }
    }
}
