using EmployeeAdministration.Interfaces;

namespace EmployeeAdministration.EventBus
{
    public class TaskEvent
    {
        private readonly IRabbitMQ _rabbitMQ;
        public TaskEvent(IRabbitMQ rabbitMQ)
        {
            _rabbitMQ = rabbitMQ;
        }
        public void LogTaskCreated(string TaskName)
        {
            string message = $"Project created: {TaskName}";
            _rabbitMQ.SendMessage(message);
        }

        public void LogTaskAssigned(List<string> userNames)
        {
            string userNamesStr = string.Join(", ", userNames);
            string message = $"Task assigned to: {userNamesStr}";
            _rabbitMQ.SendMessage(message);
        }

        public void LogTaskUpdated(string taskName)
        {
            string message = $"Task updated: {taskName}";
            _rabbitMQ.SendMessage(message);
        }
        public void LogTaskStatusUpdate(string taskName, bool isCompleted)
        {
            string message;
            if (isCompleted) 
            {
                 message = $"{taskName} is signed as completed";
            }
            else
            {
                 message = $"{taskName} is signed as incompleted";

            }
            _rabbitMQ.SendMessage(message);
        }

        public void LogTaskDeleted(string taskName)
        {
            string message = $"Task deleted: {taskName}";
            _rabbitMQ.SendMessage(message);
        }
        public void LogTaskAssigmentDeleted(string userName)
        {
            string message = $"Task removed from: {userName}";
            _rabbitMQ.SendMessage(message);
        }

    }
}
