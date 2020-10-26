using System.Threading;
using System.Threading.Tasks;
using Morse.Abstractions.Models;

namespace Morse.Abstractions.Services
{
    public interface IMessageConsumer<TMessage>
        where TMessage : MorseMessage
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}
