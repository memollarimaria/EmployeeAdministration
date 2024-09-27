using EmployeeAdministration.Interfaces;

namespace EmployeeAdministration.Services
{
	public class TaskService : ITask
	{
		public System.Threading.Tasks.Task CreateTask(Entities.Models.Task request)
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task DeleteTask(Guid taskId)
		{
			throw new NotImplementedException();
		}

		public Task<ICollection<Entities.Models.Task>> GetUserTasks()
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task UpdateTask(Entities.Models.Task request)
		{
			throw new NotImplementedException();
		}
	}
}
