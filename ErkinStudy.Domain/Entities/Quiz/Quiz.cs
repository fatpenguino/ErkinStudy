using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.Quiz
{
    public class Quiz
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}