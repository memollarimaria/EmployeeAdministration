using EmployeeAdministration.ViewModels.UserTasksViewModels;
using Entities.Models;

namespace EmployeeAdministration.ViewModels.TasksViewModels
{
	public class CreateTaskViewModel
	{
		public string TaskName { get; set; }
		public string Description { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime? DueDate { get; set; }
		public DateTime CreatedAt { get; set; }
		public Guid ProjectId { get; set; }
		public ICollection<UserTaskViewModel> UserTasks { get; set; }
	}
}
