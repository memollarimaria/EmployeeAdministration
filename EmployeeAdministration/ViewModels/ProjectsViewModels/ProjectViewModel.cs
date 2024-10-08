﻿

using EmployeeAdministration.ViewModels.TasksViewModels;
using EmployeeAdministration.ViewModels.UserProjectsViewModels;
using Entities.Enum;

namespace EmployeeAdministration.ViewModels.ProjectsViewModels
{
	public class ProjectViewModel
	{
		public string ProjectName { get; set; }
		public Status ProjectStatus { get; set; }
		public string Description { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? EndDate { get; set; }
		public ICollection<TaskViewModel> Tasks { get; set; }
	}
}
