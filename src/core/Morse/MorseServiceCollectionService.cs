using System;
using Microsoft.Extensions.DependencyInjection;

namespace Morse
{
    public static class MorseServiceCollectionService
    {
        public static IServiceCollection AddMorse(this IServiceCollection services)
        {
            return services;
        }
    }
}
