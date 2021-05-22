using System;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Morse.Abstractions.Services;
using Morse.RabbitMq.Extensions;

namespace Morse.Tests.RabbitMq
{
    [TestClass]
    public class TestRabbitMqQueues : TestContainer
    {
        private IMorsePublisher<MorseTestModel> publisher;

        [TestMethod]
        public void Publish()
        {
            publisher.Publish(new MorseTestModel
            {
                PublishTime = DateTime.Now
            });
        }

        [TestMethod]
        public void PublishConfirm()
        {
            Assert.IsTrue(ConsumedMessageCollection.Messages.Any());
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddTransient<IMessageHandler<MorseTestModel>, MorseTestModelHandler>();

            services.AddMorseRabbitMqPublishers(Configuration);
            services.AddMorseRabbitMqConsumers(Configuration);
        }

        protected override void ResolveServices()
        {
            base.ResolveServices();
            var consumer = ServiceProvider.GetService(typeof(IMorseConsumer<>).MakeGenericType(typeof(MorseTestModel)));
            consumer.GetType().GetMethod("Start")?.Invoke(consumer, new object[1] { new CancellationToken() });

            publisher = ServiceProvider.GetService<IMorsePublisher<MorseTestModel>>();
        }
    }
}
