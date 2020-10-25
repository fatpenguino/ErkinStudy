using ErkinStudy.Domain.Entities.Identity;

namespace ErkinStudy.Domain.Entities.Quizzes
{
    public class UserAnswer
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long QuestionId { get; set; }
        public string Answer { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Question Question { get; set; }
    }
}
