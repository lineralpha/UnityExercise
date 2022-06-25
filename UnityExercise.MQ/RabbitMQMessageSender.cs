using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace UnityExercise.MQ
{
    public class RabbitMQMessageSender : IMessageSender, IDisposable
    {
        private readonly ILogger<RabbitMQMessageSender> _logger;
        private readonly RabbitMQConfiguration _rabbitmqConfig;
        private IConnection _rabbitmqConnection;

        public RabbitMQMessageSender(
            IOptions<RabbitMQConfiguration> options,
            ILogger<RabbitMQMessageSender> logger)
        {
            _logger = logger;
            _rabbitmqConfig = options.Value;

            CreateConnection();
        }

        public virtual void Dispose()
        {
            _logger.LogWarning("Disposing RabbitMQMessageSender");
            _rabbitmqConnection?.Dispose();
        }

        public Task SendMessageAsync<T>(T message)
        {
            using (var channel = _rabbitmqConnection.CreateModel())
            {
                channel.QueueDeclare(_rabbitmqConfig.QueueName, durable: false, exclusive: false);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: _rabbitmqConfig.QueueName, body: body);
            }

            return Task.CompletedTask;
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _rabbitmqConfig.Hostname,
                    UserName = _rabbitmqConfig.UserName,
                    Password = _rabbitmqConfig.Password
                };

                _rabbitmqConnection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                // BrokerUnreachableException
                _logger.LogCritical("Cannot connect to RabbitMQ ({0}): {1}", _rabbitmqConfig.Hostname, ex.Message);
            }
        }

    }
}
