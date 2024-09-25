using EmployeeAdministration.DTOs.ProjectDTOs;
using EmployeeAdministration.DTOs.UserDTOs;
using Entities.Models;

namespace EmployeeAdministration.DTOs.UserProjectDTOs
{
	public class UserProjectDTO
	{
		public UserDTO User { get; set; }
		public ProjectDTO Project { get; set; }
	}
}
