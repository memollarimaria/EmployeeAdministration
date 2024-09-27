
using Abp.Events.Bus;

namespace EmployeeAdministration.EventBus.UserEventBus
{
	public class UserCreatedEvent : IEventData
	{
		public int UserId { get; }
		public string UserName { get; }
		public DateTime EventTime { get; set; }
		public object EventSource { get; set ; }

		public UserCreatedEvent(int userId, string userName)
		{
			UserId = userId;
			UserName = userName;
			EventTime = DateTime.Now;
		}
	}

}
