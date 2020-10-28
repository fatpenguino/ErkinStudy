using System.Collections.Generic;

namespace ErkinStudy.Web.Models.Quiz
{
    public class FilledQuizViewModel
    {
        public long QuizId { get; set; }
        public List<AnswerModel> Answers { get; set; }
    }

    public class AnswerModel
    {
        public string QuestionId { get; set; }
        public string Answer { get; set; }
    }
}
