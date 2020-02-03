using System.ComponentModel.DataAnnotations;

namespace ErkinStudy.Web.Models
{
	public class RegisterViewModel
	{
        [Required(ErrorMessage = "Email енгізіңіз")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Атыңызды енгізіңіз")]
        [Display(Name = "Аты")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Тегіңізді енгізіңіз")]
        [Display(Name = "Тегі")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Телефон нөміріңізді енгізіңіз")]
		[Phone]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Құпиясөзді енгізіңіз")]
		[DataType(DataType.Password)]
        [Display(Name = "Құпиясөз")]
        public string Password { get; set; }

		[Required(ErrorMessage = "Құпиясөз қайта енгізіңіз")]
		[DataType(DataType.Password)]
		[Compare("Password")]
		[Display(Name = "Құпиясөзді қайта теру")]
		public string ConfirmPassword { get; set; }

	}
}
