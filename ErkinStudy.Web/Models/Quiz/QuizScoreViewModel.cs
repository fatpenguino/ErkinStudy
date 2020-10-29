using System.Collections.Generic;

namespace ErkinStudy.Web.Models.Quiz
{
    public class QuizScoreViewModel
    {
        public long QuizId { get; set; }
        public string QuizTitle { get; set; }
        public List<AnsweredQuestion> Questions { get; set; } = new List<AnsweredQuestion>();
        public int Score { get; set; }
    }

    public class AnsweredQuestion
    {
        public long QuestionId { get; set; }
        public long AnswerId { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public string Answer { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
