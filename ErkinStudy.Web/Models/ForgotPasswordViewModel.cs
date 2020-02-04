using System.ComponentModel.DataAnnotations;

namespace ErkinStudy.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email енгізіңіз")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
