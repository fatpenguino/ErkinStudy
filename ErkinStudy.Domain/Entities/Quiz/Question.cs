using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.Quiz
{
    public class Question
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}