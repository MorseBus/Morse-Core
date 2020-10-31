using System.Threading.Tasks;
using Morse.Abstractions.Services;

namespace Morse.Tests
{
    public class MorseTestModelHandler : IMessageHandler<MorseTestModel>
    {
        public Task HandleAsync(MorseTestModel message)
        {
            ConsumedMessageCollection.Messages.Add(message);
            return Task.CompletedTask;
        }
    }
}
