using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using ErkinStudy.Domain.Entities.Quizzes;
using System.Collections.Generic;

namespace ErkinStudy.Web.Models
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