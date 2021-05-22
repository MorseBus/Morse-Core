using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Morse.Abstractions.Models;
using Morse.Abstractions.Services;

namespace Morse.Core.Services
{
    public class BinarySerializer<TMessage> : IMessageSerializer<TMessage> where TMessage : IQueueParameters
    {
        public TMessage Deserialize(byte[] bytes)
        {
            using var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return (TMessage)formatter.Deserialize(stream);
        }

        public byte[] Serialize(TMessage message)
        {
            var formatter = new BinaryFormatter();
            using var stream = new MemoryStream();
            formatter.Serialize(stream, message);
            return stream.ToArray();
        }
    }
}
