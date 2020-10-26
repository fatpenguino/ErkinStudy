namespace ErkinStudy.Domain.Entities.Quizzes
{
    public class UserAnswer
    {
        public long Id { get; set; }
        public long QuizScoreId { get; set; }
        public long QuestionId { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        public virtual QuizScore QuizScore { get; set; }
        public virtual Question Question { get; set; }
    }
}
