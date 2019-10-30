using System.ComponentModel.DataAnnotations;

namespace ErkinStudy.Web.Models
{
	public class RegisterViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password")]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }

	}
}
