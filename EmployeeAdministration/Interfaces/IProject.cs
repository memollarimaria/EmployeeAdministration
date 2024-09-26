using Entities.Models;
using Task = System.Threading.Tasks.Task;

namespace EmployeeAdministration.Interfaces
{
	public interface IProject
	{
		Task<ICollection<Project>> GetUserProjects();
		Task<ICollection<Project>> GetProjectTasks(Guid projectId);
		Task CreateProject(Project request);
		Task DeleteProject(Guid projectId);
		Task UpdateProject(Project request);
	}
}
