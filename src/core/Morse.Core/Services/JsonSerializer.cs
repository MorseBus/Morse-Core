using System.Text;
using Morse.Abstractions.Models;
using Morse.Abstractions.Services;
using Newtonsoft.Json;

namespace Morse.Core.Services
{
    public class JsonSerializer<TMessage> : IMessageSerializer<TMessage> where TMessage : IQueueParameters
    {
        public TMessage Deserialize(byte[] bytes)
            => JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(bytes));

        public byte[] Serialize(TMessage message)
            => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
    }
}
