using System.Text;
using Morse.Abstractions.Models;
using Morse.Abstractions.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Morse.RabbitMq
{

    ///TODO: 
    ///make the serializing and contnet type configurable
    ///no need to create and bind queus each time (check if the queue already exsists)
    ///enahnce connection creating
    public class RabbitMqPublisher<TMessage> : IMorsePublisher<TMessage>
        where TMessage : MorseMessage
    {
        private readonly IQueueBuilder<TMessage> queueBuilder;

        public void Publish(TMessage message)
        {

            var queueDefinition = queueBuilder.Build(message);
            using var channel = CreateConnection(queueDefinition.Connection);

            channel.ExchangeDeclare(queueDefinition.Connection.Exchange, ExchangeType.Topic, true, false);
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

        private IModel CreateConnection(ConnectionDefinition properties)
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
