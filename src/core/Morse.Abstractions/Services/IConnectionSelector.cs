using Morse.Abstractions.Models;

namespace Morse.Abstractions.Services
{
    public interface IConnectionSelector
    {
        ConnectionDefinition GetConnection(string identifier);
    }
}
