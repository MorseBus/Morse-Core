using System.Threading.Tasks;
using Morse.Abstractions.Models;

namespace Morse.Abstractions.Services
{
    public interface IMessageHandler<TMessage> where TMessage : IMorseMessage
    {
        Task HandleAsync(TMessage message);
    }
}
