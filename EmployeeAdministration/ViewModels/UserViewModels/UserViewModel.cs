using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAdministration.ViewModels.UserViewModels
{
	public class UserViewModel
	{
		public string Email { get; set; }
		public ICollection<UserProject> UserProjects { get; set; }
		public ICollection<UserTask> UserTasks { get; set; }
	}
}
