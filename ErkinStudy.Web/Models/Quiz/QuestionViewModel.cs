using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErkinStudy.Domain.Entities.Quizzes;
using Microsoft.AspNetCore.Http;

namespace ErkinStudy.Web.Models.Quiz
{
    public class QuestionViewModel
    {
        [Required]
        public string Content {get; set;}
        public IFormFile Image { get; set; }
        public long Id { get; set; }
        public long QuizId { get; set; }
        public List<Answer> Answers { get; set; }

        public QuestionViewModel()
        {
            Answers = new List<Answer>();
        }
    }
}