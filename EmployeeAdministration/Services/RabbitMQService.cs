using EmployeeAdministration.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace EmployeeAdministration.Services
{
    public class RabbitMQService  : IRabbitMQ
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly Serilog.ILogger _logger;

        public RabbitMQService(Serilog.ILogger logger)
        {
            _factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "user_events", type: ExchangeType.Fanout);
            _logger = logger;
        }
        public async Task SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "user_events",
                                  routingKey: "",
                                  basicProperties: null,
                                  body: body);
            _logger.Information("Message published: {Message}", message);
        }

        public async Task ReceiveLog()
        {
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                               exchange: "user_events",
                               routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.Information("Received: {Message}", message);
            };

            _channel.BasicConsume(queue: queueName,
                                  autoAck: true,
                                  consumer: consumer);

        }

        public void Close()
        {
            _connection.Close();
        }
    }
}
