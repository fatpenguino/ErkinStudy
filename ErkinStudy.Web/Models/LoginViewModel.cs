using System.ComponentModel.DataAnnotations;

namespace ErkinStudy.Web.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Email енгізіңіз")]
        [Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Құпиясөзді енгізіңіз")]
        [Display(Name = "Құпиясөз")]
        [DataType(DataType.Password)]
		public string Password { get; set; }

	}
}
