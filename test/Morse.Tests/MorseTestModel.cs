using System;
using Morse.Abstractions.Models;

namespace Morse.Tests
{
    public class MorseTestModel : MorseMessage
    {
        public DateTime PublishTime { get; set; }
    }
}
