﻿using System.Threading;
using System.Threading.Tasks;
using Morse.Abstractions.Models;

namespace Morse.Abstractions.Services
{
    public interface IMorseConsumer<TMessage>
        where TMessage : MorseMessage
    {
        Task Start(CancellationToken cancellationToken);
    }
}
