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
		Task CreateUserTask(CreateTaskViewModel request);
		Task UpdateUserTaskStatus(Guid taskId);
		Task AssignTaskTo(AssignTaskViewModel request);
		Task UserAssignTaskTo(AssignTaskViewModel request);
		Task RemoveAssignment(RemoveAssignmentViewModel request);

    }
}
