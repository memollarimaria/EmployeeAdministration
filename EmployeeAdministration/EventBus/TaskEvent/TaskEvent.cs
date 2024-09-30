using Abp.Events.Bus;
using System;
using System.Collections.Generic;

namespace EmployeeAdministration.Events
{
	public class TaskCreatedEvent : EventData
	{
		public Guid TaskId { get; set; }
		public string TaskName { get; set; }
	}

	public class TaskUpdatedEvent : EventData
	{
		public Guid TaskId { get; set; }
		public string TaskName { get; set; }
	}

	public class TaskDeletedEvent : EventData
	{
		public Guid TaskId { get; set; }
		public string TaskName { get; set; }
	}

	public class TaskAssignedEvent : EventData
	{
		public Guid TaskId { get; set; }
		public List<Guid> AssignedUserIds { get; set; }
	}

	public class TaskStatusUpdatedEvent : EventData
	{
		public Guid TaskId { get; set; }
		public bool IsCompleted { get; set; }
	}
}
