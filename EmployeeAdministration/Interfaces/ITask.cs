using EmployeeAdministration.ViewModels.TasksViewModels;
using System.Threading.Tasks;

namespace EmployeeAdministration.Interfaces
{
	public interface ITask
	{
		Task<ICollection<TaskViewModel>> GetUserTasks();
		Task CreateTask(CreateTaskViewModel request);
		Task DeleteTask(Guid taskId);
		Task UpdateTask(UpdateTaskViewModel request);
		Task UpdateTaskStatus(Guid taskId);
		Task<ICollection<TaskViewModel>> GetAllTasks();
	}
}
