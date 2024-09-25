using EmployeeAdministration.DTOs.TaskDTOs;

namespace EmployeeAdministration.DTOs.ProjectDTOs
{
	public class ProjectDTO
	{
		public string ProjectName { get; set; }
		public ICollection<TaskDTO> Tasks { get; set; }
	}
}
