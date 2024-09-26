

using EmployeeAdministration.ViewModels.TasksViewModels;

namespace EmployeeAdministration.ViewModels.ProjectsViewModels
{
	public class ProjectViewModel
	{
		public string ProjectName { get; set; }
		public ICollection<TaskViewModel> Tasks { get; set; }
	}
}
