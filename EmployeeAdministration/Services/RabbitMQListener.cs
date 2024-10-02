using EmployeeAdministration.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

public class RabbitMQListener : BackgroundService
{
    private readonly IRabbitMQ _rabbitMQ;

    public RabbitMQListener(IRabbitMQ rabbitMQ)
    {
        _rabbitMQ = rabbitMQ;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _rabbitMQ.ReceiveLog();
    }
}
