using EmployeeAdministration.Interfaces;
using Entities.Models;

namespace EmployeeAdministration.Services
{
	public class ProjectService : IProject
	{
		public System.Threading.Tasks.Task CreateProject(Project request)
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task DeleteProject(Guid projectId)
		{
			throw new NotImplementedException();
		}

		public Task<ICollection<Project>> GetProjectTasks(Guid projectId)
		{
			throw new NotImplementedException();
		}

		public Task<ICollection<Project>> GetUserProjects()
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task UpdateProject(Project request)
		{
			throw new NotImplementedException();
		}
	}
}
