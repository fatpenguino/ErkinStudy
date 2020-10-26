using System;
using System.Collections.Generic;
using ErkinStudy.Domain.Entities.Identity;

namespace ErkinStudy.Domain.Entities.Quizzes
{
    public class QuizScore
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long QuizId { get; set; }
        public int Point { get; set; }
        public DateTime TakenTime { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }

}