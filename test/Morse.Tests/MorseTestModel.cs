using System;
using Morse.Abstractions.Models;

namespace Morse.Tests
{
    public class MorseTestModel : IMorseMessage
    {
        public DateTime PublishTime { get; set; }
    }
}
