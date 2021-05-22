using System.Threading.Tasks;
using Morse.Abstractions.Models;

namespace Morse.Abstractions.Services
{
    public interface IMorsePublisher<TMessage> where TMessage : IQueueParameters
    {
        void Publish(TMessage message);
    }
}
