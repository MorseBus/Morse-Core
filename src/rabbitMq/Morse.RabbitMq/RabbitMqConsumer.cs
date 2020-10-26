using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Morse.Abstractions.Models;
using Morse.Abstractions.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Morse.RabbitMq
{
    ///TODO: 
    ///make the serializing configurable
    public class RabbitMqConsumer<TMessage> : IMorseConsumer<TMessage>
        where TMessage : MorseMessage
    {
        private readonly IQueueBuilder<TMessage> queueBuilder;
        private readonly IServiceProvider serviceProvider;
        public RabbitMqConsumer(
            IQueueBuilder<TMessage> queueBuilder,
            IServiceProvider serviceProvider)
        {
            this.queueBuilder = queueBuilder;
            this.serviceProvider = serviceProvider;
        }

        public void Start(CancellationToken cancellationToken)
        {
            foreach (var queueDefinition in queueBuilder.Build())
            {
                Task.Run(() =>
                {
                    using var channel = CreateConnection(queueDefinition.Connection);

                    channel.ExchangeDeclare(queueDefinition.Connection.Exchange, ExchangeType.Topic, durable: true, autoDelete: false);
                    channel.QueueDeclare(queueDefinition.Name, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueBind(queueDefinition.Name, queueDefinition.Connection.Exchange, queueDefinition.RoutingKey);
                    var consumer = new AsyncEventingBasicConsumer(channel);

                    consumer.Received += Consumer_Received;
                });
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var message = JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(eventArgs.Body.ToArray()));
            foreach (var handler in serviceProvider.CreateScope().ServiceProvider.GetServices<IMessageHandler<TMessage>>())
            {
                await handler.HandleAsync(message);
            }
            ((EventingBasicConsumer)sender).Model.BasicAck(eventArgs.DeliveryTag, false);
        }

        private IModel CreateConnection(ConnectionDefinition properties)
        {
            var connection = new ConnectionFactory()
            {
                HostName = properties.Host,
                VirtualHost = properties.Host,
                Port = properties.Port,
                UserName = properties.Username,
                Password = properties.Password
            }.CreateConnection();

            var channel = connection.CreateModel();
            return channel;
        }
    }
}
