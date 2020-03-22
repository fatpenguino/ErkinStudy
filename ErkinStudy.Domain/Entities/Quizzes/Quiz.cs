using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.Quizzes
{
    public class Quiz
    {
        public long Id { get; set; }
        public long? FolderId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public int Price { get; set; }
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<UserQuiz> UserQuizzes { get; set; }
    }
}