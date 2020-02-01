using System.ComponentModel.DataAnnotations;

namespace ErkinStudy.Web.Models
{
	public class LoginViewModel
	{
		[Required]
        [Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
        [Display(Name = "Құпиясөз")]
        [DataType(DataType.Password)]
		public string Password { get; set; }

	}
}
