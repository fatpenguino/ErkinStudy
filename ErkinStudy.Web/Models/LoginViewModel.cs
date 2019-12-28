using System.ComponentModel.DataAnnotations;

namespace ErkinStudy.Web.Models
{
	public class LoginViewModel
	{
		[Required]
        [Display(Name = "Логин")]
		public string Username { get; set; }

		[Required]
        [Display(Name = "Құпиясөз")]
        [DataType(DataType.Password)]
		public string Password { get; set; }

	}
}
