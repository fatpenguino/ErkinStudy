namespace ErkinStudy.Web.Models
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsApprovedOnlineCourse { get; set; }
    }
}
