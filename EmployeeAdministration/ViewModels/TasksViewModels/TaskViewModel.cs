﻿namespace EmployeeAdministration.ViewModels.TasksViewModels
{
	public class TaskViewModel
	{
		public string TaskName { get; set; }
		public string Description { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime? DueDate { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
