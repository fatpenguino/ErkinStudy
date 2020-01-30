namespace ErkinStudy.Domain.Entities.Quizzes
{
    public class Answer
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
        public long QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}