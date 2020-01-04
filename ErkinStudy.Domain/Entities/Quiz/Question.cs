using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.Quiz
{
    public class Question
    {
        public long Id { get; set; }
        public string Content {get; set;}
        public virtual ICollection<Answer> Answers { get; set; }
    }
}