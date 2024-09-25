using EmployeeAdministration.DTOs.UserProjectDTOs;
using EmployeeAdministration.DTOs.UserTaskDTOs;
using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAdministration.DTOs.UserDTOs
{
	public class UserDTO
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		public ICollection<UserProjectDTO> UserProjects { get; set; }
		public ICollection<UserTaskDTO> UserTasks { get; set; }
	}
}
