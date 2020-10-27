using System.Collections.Generic;

namespace ErkinStudy.Web.Models.Quiz
{
    public class UserAnswerViewModel
    {
        public long QuestionId { get; set; }
        public long QuizId { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public string CorrectAnswer { get; set; }
        public List<FilledAnswer> FilledAnswers { get; set; } = new List<FilledAnswer>();
    }

    public class FilledAnswer
    {
        public long UserAnswerId { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        
    }
}
