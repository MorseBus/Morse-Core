using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Morse.Abstractions.Services;
using Morse.RabbitMq.Extensions;

namespace Morse.Tests.RabbitMq
{
    public class TestRabbitMqPublisher : TestContainer
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

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddMorseRabbitMqPublisher(Configuration);
        }

        protected override void ResolveServices()
        {
            base.ResolveServices();
            publisher = ServiceProvider.GetService<IMorsePublisher<MorseTestModel>>();
        }
    }
}
