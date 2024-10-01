using Abp.Events.Bus.Handlers;
using EmployeeAdministration.EventsBus.UserEventHandler;
using Serilog;

namespace EmployeeAdministration.EventHandlers
{
	public class UserCreatedEventHanlder : IEventHandler<UserCreatedEvent>
	{
		public void HandleEvent(UserCreatedEvent eventData)
		{
			Console.WriteLine($"User created: {eventData.Email}");
			
		}
	}
	public class UserLoggedInEventHandler : IEventHandler<UserLoggedInEvent>
	{
		private readonly ILogger<UserLoggedInEventHandler> _logger;

		public UserLoggedInEventHandler(ILogger<UserLoggedInEventHandler> logger)
		{
			_logger = logger;
		}
		public void HandleEvent(UserLoggedInEvent eventData)
		{
			_logger.LogInformation($"User logged in: {eventData.Email}");
		}
	}
	public class UserProfilePictureUpdatedEventHandler : IEventHandler<UserProfilePictureUpdatedEvent>
	{
		public void HandleEvent(UserProfilePictureUpdatedEvent eventData)
		{
			Console.WriteLine($"User profile picture updated for user ID: {eventData.UserId}");
		}
	}

	public class UserDeletedEventHandler : IEventHandler<UserDeletedEvent>
	{
		public void HandleEvent(UserDeletedEvent eventData)
		{
			Console.WriteLine($"User deleted: {eventData.Email}");
		}
	}
}
