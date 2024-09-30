using Abp.Events.Bus;
using System;
using System.Collections.Generic;

namespace EmployeeAdministration.Events
{
	public class ProjectCreatedEvent : EventData
	{
		public Guid ProjectId { get; set; }
		public string ProjectName { get; set; }
	}

	public class ProjectUpdatedEvent : EventData
	{
		public Guid ProjectId { get; set; }
		public string ProjectName { get; set; }
	}

	public class ProjectDeletedEvent : EventData
	{
		public Guid ProjectId { get; set; }
		public string ProjectName { get; set; }
	}

	public class ProjectAssignedEvent : EventData
	{
		public Guid ProjectId { get; set; }
		public List<Guid> AssignedUserIds { get; set; }
	}
}
