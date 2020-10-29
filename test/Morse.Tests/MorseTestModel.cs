using System;
using Morse.Abstractions.Models;

namespace Morse.Tests
{
    class MorseTestModel : MorseMessage
    {
        public DateTime PublishTime { get; set; }
    }
}
