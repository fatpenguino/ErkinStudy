using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.Quizzes
{
    public class Quiz
    {
        public long Id { get; set; }
        public int? CategoryId { get; set; }
        public long? FolderId { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public int Price { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<UserQuiz> UserQuizzes { get; set; }
    }
}