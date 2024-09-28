using EmployeeAdministration.ViewModels.UserTasksViewModels;

namespace EmployeeAdministration.ViewModels.TasksViewModels
{
	public class UpdateTaskViewModel
	{
		public Guid TaskId { get; set; }
		public string TaskName { get; set; }
		public string Description { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime? DueDate { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
