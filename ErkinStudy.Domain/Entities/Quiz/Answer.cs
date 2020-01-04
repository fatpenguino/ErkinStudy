namespace ErkinStudy.Domain.Entities.Quiz
{
    public class Answer
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
    }
}