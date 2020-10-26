using Microsoft.Extensions.Options;
using Morse.Abstractions.Models;
using Morse.Abstractions.Services;
using Morse.Core.Options;

namespace Morse.Core.Services
{
    public class ConnectionSelector : IConnectionSelector
    {
        protected const string DefaultConnection = "DefaultConnection";
        private readonly MorseConnectionOptions connectionOptions;

        public ConnectionSelector(IOptions<MorseConnectionOptions> options)
        {
            this.connectionOptions = options.Value;

            if (connectionOptions == null || 
                connectionOptions.Connections == null || 
                !connectionOptions.Connections.ContainsKey(DefaultConnection))
                throw new System.Exception("Connections Must contains DefaultConnection at least");
        }

        public ConnectionDefinition GetConnection(string identifier)
        {
            if (connectionOptions.Connections.TryGetValue(identifier, out var connection))
                return connection;
            return connectionOptions.Connections[DefaultConnection];
        }
    }
}