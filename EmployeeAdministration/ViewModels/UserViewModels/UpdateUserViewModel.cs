using System.ComponentModel.DataAnnotations;

namespace EmployeeAdministration.ViewModels.UserViewModels
{
	public class UpdateUserViewModel
	{
		public string PhotoPath { get; set; } = null!;
		public byte[]? PhotoContent { get; set; }
	}
}
