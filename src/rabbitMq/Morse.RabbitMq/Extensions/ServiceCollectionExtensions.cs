using Microsoft.Extensions.Configuration;
using Morse.Abstractions.Services;
using Morse.Core.Options;
using Morse.Core.Services;
using Morse.RabbitMq;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMorseRabbitMqPublishers(this IServiceCollection services, IConfiguration configuration)
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
