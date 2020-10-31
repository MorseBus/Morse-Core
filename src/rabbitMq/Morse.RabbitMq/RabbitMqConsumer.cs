using System;
using System.Linq;
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

        public Task Start(CancellationToken cancellationToken)
        {
            foreach (var queueDefinition in queueBuilder.Build())
            {
                Task.Run(() => { InitializeNewConsumer(queueDefinition); }, cancellationToken);
            }
            return Task.CompletedTask;
        }

        private void InitializeNewConsumer(QueueDefinition definition)
        {
            var channel = CreateConnection(definition.Connection);

            channel.ExchangeDeclare(definition.Connection.Exchange, ExchangeType.Topic, durable: true, autoDelete: false);
            channel.QueueDeclare(definition.Name, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(definition.Name, definition.Connection.Exchange, definition.RoutingKey);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += Consumer_Received;
            channel.BasicConsume(definition.Name, false, consumer);
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var message = JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(eventArgs.Body.ToArray()));
            foreach (var handler in serviceProvider.CreateScope().ServiceProvider.GetServices<IMessageHandler<TMessage>>())
            {
                handler.HandleAsync(message).Wait();
            }
            ((EventingBasicConsumer)sender).Model.BasicAck(eventArgs.DeliveryTag, false);
        }

        private static IModel CreateConnection(ConnectionDefinition properties)
        {
            var connection = new ConnectionFactory()
            {
                HostName = properties.Host,
                VirtualHost = properties.VirtualHost,
                Port = properties.Port,
                UserName = properties.Username,
                Password = properties.Password
            }.CreateConnection();

            var channel = connection.CreateModel();
            return channel;
        }
    }
}
