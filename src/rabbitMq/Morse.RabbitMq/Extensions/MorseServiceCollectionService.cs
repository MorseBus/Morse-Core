using Microsoft.Extensions.DependencyInjection;
using Morse.Abstractions.Services;
using Morse.Core.Services;

namespace Morse.RabbitMq.Extensions
{
    public static class MorseServiceCollectionService
    {
        public static IServiceCollection AddMorsePublisher(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IConnectionSelector), typeof(ConnectionSelector));
            services.AddSingleton(typeof(IQueueBuilder<>), typeof(QueueBuilder<>));
            services.AddSingleton(typeof(IMorsePublisher<>), typeof(RabbitMqPublisher<>));
            return services;
        }

        public static IServiceCollection AddMorseConsumers(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IConnectionSelector), typeof(ConnectionSelector));
            services.AddSingleton(typeof(IQueueBuilder<>), typeof(QueueBuilder<>));
            services.AddSingleton(typeof(IMorseConsumer<>), typeof(RabbitMqConsumer<>));
            return services;
        }
    }
}
