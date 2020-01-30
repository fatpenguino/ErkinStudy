using System.ComponentModel.DataAnnotations;

namespace ErkinStudy.Web.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Құпиясөз")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Құпиясөзді қайта теру")]
        [Compare("Password", ErrorMessage = "Құпиясөздер сәйкес келмейді.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
