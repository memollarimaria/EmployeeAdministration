using EmployeeAdministration.ViewModels.ProjectsViewModels;
using EmployeeAdministration.ViewModels.TasksViewModels;
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
		Task<ICollection<TaskViewModel>> GetProjectTasks(Guid projectId);
		Task UpdateProject(UpdateProjectViewModel request);
		Task AssignProjectTo(AssignProjectViewModel request);
	}
}
