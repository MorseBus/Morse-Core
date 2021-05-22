using Morse.Abstractions.Models;

namespace Morse.Abstractions.Services
{
    public interface IMessageSerializer<TMessage> where TMessage : IQueueParameters
    {
        TMessage Deserialize(byte[] bytes);
        byte[] Serialize(TMessage message);
    }
}
