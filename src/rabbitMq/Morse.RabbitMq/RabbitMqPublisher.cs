using System.Text;
using Morse.Abstractions.Models;
using Morse.Abstractions.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Morse.RabbitMq
{
    public class RabbitMqPublisher<TMessage> : IMorsePublisher<TMessage>
        where TMessage : IQueueParameters
    {

        private readonly IQueueBuilder<TMessage> queueBuilder;

        public RabbitMqPublisher(IQueueBuilder<TMessage> queueBuilder)
        {
            this.queueBuilder = queueBuilder;
        }

        public void Publish(TMessage message)
        {

            var queueDefinition = queueBuilder.Build(message);
            using var channel = CreateConnection(queueDefinition.Connection);

            channel.ExchangeDeclare(queueDefinition.Connection.Exchange, ExchangeType.Topic, durable: true, autoDelete: false);
            channel.QueueDeclare(queueDefinition.Name, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queueDefinition.Name, queueDefinition.Connection.Exchange, queueDefinition.RoutingKey);

            var basicProperties = channel.CreateBasicProperties();
            basicProperties.ContentType = "application/json";
            basicProperties.DeliveryMode = 2;

            channel.BasicPublish(
                queueDefinition.Connection.Exchange,
                queueDefinition.RoutingKey,
                basicProperties,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
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
