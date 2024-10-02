using EmployeeAdministration.Helpers;
using EmployeeAdministration.Interfaces;
using EmployeeAdministration.Services;

namespace EmployeeAdministration.EventBus
{
    public class UserEvent
    {
        private readonly IRabbitMQ _rabbitMQ;
        public UserEvent(IRabbitMQ rabbitMQ)
        {
            _rabbitMQ = rabbitMQ;
        }
        public void LogUserCreated(string userEmail)
        {
            string message = $"User created: {userEmail}";
            _rabbitMQ.SendMessage(message);
        }

        public void LogUserLoggedIn(string userEmail)
        {
            string message = $"User logged in: {userEmail}";
            _rabbitMQ.SendMessage(message);
        }

        public void LogUserProfileUpdated(string userEmail)
        {
            string message = $"User profile updated: {userEmail}";
            _rabbitMQ.SendMessage(message);
        } 
        
        public void LogUserDeleted(string userEmail)
        {
            string message = $"User profile deleted: {userEmail}";
            _rabbitMQ.SendMessage(message);
        }
    }
}
