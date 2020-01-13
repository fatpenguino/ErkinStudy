namespace ErkinStudy.Domain.Entities.Quiz
{
    public class Score
    {
        public long UserId { get; set; }
        public AccomplishmentType AccomplishmentType { get; set; }
        public long AssignmentId { get; set; }
        public int Point { get; set; }
    }

    public enum AccomplishmentType
    {
        Lesson,
        Quiz
    }
}