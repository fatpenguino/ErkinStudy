using System.Collections.Generic;
using ErkinStudy.Domain.Enums;

namespace ErkinStudy.Domain.Entities.Quizzes
{
    public class Quiz
    {
        public long Id { get; set; }
        public QuizType Type { get; set; }
        public long? FolderId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public string Color { get; set; }
        public QuizStatus Status { get; set; }
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}