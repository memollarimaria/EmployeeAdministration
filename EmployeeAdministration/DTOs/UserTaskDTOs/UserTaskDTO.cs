using EmployeeAdministration.DTOs.TaskDTOs;
using EmployeeAdministration.DTOs.UserDTOs;
using Entities.Models;

namespace EmployeeAdministration.DTOs.UserTaskDTOs
{
	public class UserTaskDTO
	{
		public TaskDTO Task { get; set; }
		public UserDTO User { get; set; }

	}
}
