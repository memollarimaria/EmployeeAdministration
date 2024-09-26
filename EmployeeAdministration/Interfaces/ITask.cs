using System.Threading.Tasks;

namespace EmployeeAdministration.Interfaces
{
	public interface ITask
	{
		Task<ICollection<Entities.Models.Task>> GetUserTasks();
		Task CreateTask(Entities.Models.Task request);
		Task DeleteTask(Guid taskId);
		Task UpdateTask(Entities.Models.Task request);
	}
}
