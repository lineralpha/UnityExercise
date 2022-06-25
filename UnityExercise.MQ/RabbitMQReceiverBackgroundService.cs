using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnityExercise.MQ
{
    public sealed class RabbitMQReceiverBackgroundService : BackgroundService
    {
        private readonly RabbitMQConfiguration _rabbitmqConfig;
        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<RabbitMQReceiverBackgroundService> _logger;
        private IModel _channel;
        private IConnection _rabbitmqConnection;

        public RabbitMQReceiverBackgroundService(
            ILogger<RabbitMQReceiverBackgroundService> logger,
            IOptions<RabbitMQConfiguration> options,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _rabbitmqConfig = options.Value;
            _serviceProvider = serviceProvider;

            InitializeRabbitMQListener();
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _rabbitmqConnection?.Dispose();
            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, e) =>
            {
                var content = Encoding.UTF8.GetString(e.Body.ToArray());
                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    IMessageReceiver receiver = scope.ServiceProvider.GetRequiredService<IMessageReceiver>();
                    await receiver.HandleMessageAsync(content);
                }
                _channel.BasicAck(e.DeliveryTag, multiple: false);
            };

            consumer.Shutdown += (s, e) => { };
            consumer.Registered += (s, e) => { };
            consumer.Unregistered += (s, e) => { };

            _channel.BasicConsume(_rabbitmqConfig.QueueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        private void InitializeRabbitMQListener()
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
                _rabbitmqConnection.ConnectionShutdown += (sender, e) => { };

                _channel = _rabbitmqConnection.CreateModel();
                _channel.QueueDeclare(_rabbitmqConfig.QueueName, durable: false, exclusive: false);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Cannot connect to RabbitMQ ({0}): {1}", _rabbitmqConfig.Hostname, ex.Message);
            }
        }
    }
}
