using System.Collections.Generic;
using Morse.Abstractions.Models;

namespace Morse.Core.Options
{
    public class MorseConnectionOptions
    {
        public Dictionary<string, ConnectionDefinition> Connections { get; set; }
    }
}