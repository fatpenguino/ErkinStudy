using System.ComponentModel.DataAnnotations;

namespace ErkinStudy.Web.Models
{
	public class RegisterViewModel
	{
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Аты")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Тегі")]
        public string LastName { get; set; }

        [Required]
		[Phone]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Required]
		[DataType(DataType.Password)]
        [Display(Name = "Құпиясөз")]
        public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password")]
		[Display(Name = "Құпиясөзді қайта теру")]
		public string ConfirmPassword { get; set; }

	}
}
