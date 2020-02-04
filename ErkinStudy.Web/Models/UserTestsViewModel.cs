using ErkinStudy.Domain.Entities.Quizzes;

namespace ErkinStudy.Web.Models
{
    public class UserTestsViewModel
    {
        public Quiz Quiz { get; set; }
        public bool IsApproved { get; set; }
    }
}