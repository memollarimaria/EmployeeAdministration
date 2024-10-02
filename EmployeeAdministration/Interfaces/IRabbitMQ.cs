namespace EmployeeAdministration.Interfaces
{
    public interface IRabbitMQ
    {
        Task SendMessage(string message);
        Task ReceiveLog();
    }
}
