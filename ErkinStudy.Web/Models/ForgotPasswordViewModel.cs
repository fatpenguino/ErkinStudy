using System.ComponentModel.DataAnnotations;

namespace ErkinStudy.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
