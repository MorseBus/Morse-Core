using System;
using Morse.Abstractions.Models;

namespace Morse.Tests
{
    public class MorseTestModel : IQueueParameters
    {
        public DateTime PublishTime { get; set; }
    }
}
