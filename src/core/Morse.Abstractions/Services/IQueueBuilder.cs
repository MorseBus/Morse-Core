using System.Collections.Generic;
using Morse.Abstractions.Models;

namespace Morse.Abstractions.Services
{
    public interface IQueueBuilder<TMessage> where TMessage : IMorseMessage
    {
        IEnumerable<QueueDefinition> Build();
        QueueDefinition Build(TMessage message);
    }
}