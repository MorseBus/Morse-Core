using System.Collections.Generic;
using Morse.Abstractions.Models;
using Morse.Abstractions.Services;

namespace Morse.Core.Services
{
    public class QueueBuilder<TMessage> : IQueueBuilder<TMessage> where TMessage : IQueueParameters
    {
        private readonly IConnectionSelector connectionSelector;

        public QueueBuilder(IConnectionSelector connectionSelector)
        {
            this.connectionSelector = connectionSelector;
        }

        public IEnumerable<QueueDefinition> Build()
        {
            yield return new QueueDefinition
            {
                Name = typeof(TMessage).Name,
                RoutingKey = $"{typeof(TMessage).Name}.*",
                Connection = connectionSelector.GetConnection(typeof(TMessage).Name)
            };
        }

        public QueueDefinition Build(TMessage message)
        {
            return new QueueDefinition
            {
                Name = typeof(TMessage).Name,
                RoutingKey = $"{typeof(TMessage).Name}.*",
                Connection = connectionSelector.GetConnection(typeof(TMessage).Name)
            };
        }
    }
}
