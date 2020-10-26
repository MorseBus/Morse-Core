using System.Threading.Tasks;
using Morse.Abstractions.Models;

namespace Morse.Abstractions.Services
{
    public interface IMessegPublisher<TMessage>
        where TMessage : MorseMessage
    {
        Task PublishAsync(TMessage message);
    }
}
