using ErkinStudy.Domain.Entities.Identity;

namespace ErkinStudy.Domain.Entities.Quizzes
{
    public class UserQuiz
    {
        public long UserId { get; set; }
        public long QuizId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
