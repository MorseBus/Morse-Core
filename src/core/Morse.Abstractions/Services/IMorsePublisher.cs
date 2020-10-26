using System.Threading.Tasks;
using Morse.Abstractions.Models;

namespace Morse.Abstractions.Services
{
    public interface IMorsePublisher<TMessage>
        where TMessage : MorseMessage
    {
        void Publish(TMessage message);
    }
}
