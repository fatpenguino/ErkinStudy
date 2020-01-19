using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErkinStudy.Domain.Entities.Quiz;
using Microsoft.AspNetCore.Http;

namespace ErkinStudy.Web.Models
{
    public class QuestionCreateViewModel
    {
        [Required]
        public string Content {get; set;}
        public IFormFile Photo { get; set; }
        public long QuizId { get; set; }

    }
}