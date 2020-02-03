using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ErkinStudy.Web.Models
{
    public class QuestionViewModel
    {
        [Required]
        public string Content {get; set;}
        public IFormFile Image { get; set; }
        public long Id { get; set; }
        public long QuizId { get; set; }

    }
}