using System.Threading.Tasks;
using Morse.Abstractions.Models;

namespace Morse.Abstractions.Services
{
    public interface IMessageHandler<TMessage>
        where TMessage : MorseMessage
    {
        Task HandleAsync(TMessage message);
    }
}
