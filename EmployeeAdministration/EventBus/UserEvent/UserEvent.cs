using Abp.Events.Bus;
using System;

namespace EmployeeAdministration.EventsBus.UserEventHandler
{
	public class UserCreatedEvent : EventData
	{
		public Guid UserId { get; set; }
		public string Email { get; set; }
	}

	public class UserLoggedInEvent : EventData
	{
		public Guid UserId { get; set; }
		public string Email { get; set; }
	}

	public class UserProfilePictureUpdatedEvent : EventData
	{
		public Guid UserId { get; set; }
		public byte[] PhotoContent { get; set; }
	}

	public class UserDeletedEvent : EventData
	{
		public Guid UserId { get; set; }
		public string Email { get; set; }
	}
}
