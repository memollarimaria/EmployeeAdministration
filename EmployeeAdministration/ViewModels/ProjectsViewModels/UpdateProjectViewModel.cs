using EmployeeAdministration.ViewModels.UserProjectsViewModels;
using Entities.Enum;
using Entities.Models;

namespace EmployeeAdministration.ViewModels.ProjectsViewModels
{
	public class UpdateProjectViewModel
	{
		public Guid ProjectId { get; set; }
		public string ProjectName { get; set; }
		public string Description { get; set; }
		public DateTime EndDate { get; set; }
		public Status ProjectStatus { get; set; }
	}
}
