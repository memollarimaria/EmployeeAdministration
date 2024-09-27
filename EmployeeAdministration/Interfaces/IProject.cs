using EmployeeAdministration.ViewModels.ProjectsViewModels;
using Entities.Models;
using Task = System.Threading.Tasks.Task;

namespace EmployeeAdministration.Interfaces
{
	public interface IProject
	{
		Task<ICollection<ProjectViewModel>> GetUserProjects();
		Task<ICollection<ProjectViewModel>> GetAllProject();
		Task CreateProject(CreateProjectViewModel request);
		Task DeleteProject(Guid projectId);
		Task UpdateProject(UpdateProjectViewModel request);
	}
}
