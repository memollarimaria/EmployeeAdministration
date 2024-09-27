using EmployeeAdministration.ViewModels.UserProjectsViewModels;
using Entities.Enum;
using Entities.Models;

namespace EmployeeAdministration.ViewModels.ProjectsViewModels
{
	public class CreateProjectViewModel
	{
		public string ProjectName { get; set; }
		public string Description { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? EndDate { get; set; }
		public ICollection<UserProjectViewModel> UserProjects { get; set; }
	}
}
