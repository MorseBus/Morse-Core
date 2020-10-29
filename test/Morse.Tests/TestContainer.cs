using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Morse.Tests
{
    public class TestContainer
    {
        protected TestContainer()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json")
                .Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            ResolveServices();
        }

        protected IServiceProvider ServiceProvider { get; private set; }
        protected IConfiguration Configuration { get; private set; }


        protected virtual void ConfigureServices(IServiceCollection services) { }
        protected virtual void ResolveServices() { }
    }
}
