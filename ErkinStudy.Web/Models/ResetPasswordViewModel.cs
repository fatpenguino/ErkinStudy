using System.ComponentModel.DataAnnotations;

namespace ErkinStudy.Web.Models
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Email енгізіңіз")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Құпиясөзді енгізіңіз")]
        [Display(Name = "Құпиясөз")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Құпиясөзді қайта енгізіңіз")]
        [DataType(DataType.Password)]
        [Display(Name = "Құпиясөзді қайта теру")]
        [Compare("Password", ErrorMessage = "Құпиясөздер сәйкес келмейді.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
