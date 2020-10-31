using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Morse.Abstractions.Services;
using Morse.Core.Options;
using Morse.Core.Services;

namespace Morse.RabbitMq.Extensions
{
    public static class MorseServiceCollectionService
    {
        public static IServiceCollection AddMorseRabbitMqPublisher(this IServiceCollection services, IConfiguration configuration)
        {
            return ConfigureRabbitMqServices(services, configuration);
        }

        public static IServiceCollection AddMorseRabbitMqConsumers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(typeof(IMorseConsumer<>), typeof(RabbitMqConsumer<>));
            return ConfigureRabbitMqServices(services, configuration);
        }

        private static IServiceCollection ConfigureRabbitMqServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MorseConnectionOptions>(options => configuration.GetSection(nameof(MorseConnectionOptions)).Bind(options));
            services.AddSingleton(typeof(IConnectionSelector), typeof(ConnectionSelector));
            services.AddSingleton(typeof(IQueueBuilder<>), typeof(QueueBuilder<>));
            services.AddSingleton(typeof(IMorsePublisher<>), typeof(RabbitMqPublisher<>));
            return services;
        }
    }
}
