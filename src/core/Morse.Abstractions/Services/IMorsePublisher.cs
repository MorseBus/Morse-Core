using System.Threading.Tasks;
using Morse.Abstractions.Models;

namespace Morse.Abstractions.Services
{
    public interface IMorsePublisher<TMessage> where TMessage : IMorseMessage
    {
        void Publish(TMessage message);
    }
}
