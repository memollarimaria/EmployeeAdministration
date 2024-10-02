using EmployeeAdministration.Interfaces;

namespace EmployeeAdministration.EventBus
{
    public class ProjectEvent
    {
        private readonly IRabbitMQ _rabbitMQ;
        public ProjectEvent(IRabbitMQ rabbitMQ)
        {
            _rabbitMQ = rabbitMQ;
        }
        public void LogProjectCreated(string projectName)
        {
            string message = $"Project created: {projectName}";
            _rabbitMQ.SendMessage(message);
        }

        public void LogProjectAssigned(List<string> userNames)
        {
            string userNamesStr = string.Join(", ", userNames); 
            string message = $"Project assigned to: {userNamesStr}";
            _rabbitMQ.SendMessage(message);
        }

        public void LogProjectUpdated(string projectName)
        {
            string message = $"Project updated: {projectName}";
            _rabbitMQ.SendMessage(message);
        }

        public void LogProjectDeleted(string projectName)
        {
            string message = $"User profile deleted: {projectName}";
            _rabbitMQ.SendMessage(message);
        }
    }
}
