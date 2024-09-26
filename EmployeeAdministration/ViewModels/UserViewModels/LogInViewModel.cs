using System.ComponentModel.DataAnnotations;

namespace EmployeeAdministration.ViewModels.UserViewModels
{
	public class LogInViewModel
	{
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
