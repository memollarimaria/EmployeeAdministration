namespace EmployeeAdministration.ViewModels.TasksViewModels
{
	public class AssignTaskViewModel
	{
		public Guid TaskId { get; set; }
		public List<Guid> UserIds { get; set; }
	}
}
