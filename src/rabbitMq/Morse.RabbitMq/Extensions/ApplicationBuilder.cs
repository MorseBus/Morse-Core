using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Morse.Abstractions.Models;
using Morse.Abstractions.Services;

namespace Morse.RabbitMq.Extensions
{
    public static class ApplicationBuilder
    {
        public static IApplicationBuilder UseMorseRabbitMqConsumers(this IApplicationBuilder app, params Type[] messageAssemblyMarkerTypes)
        {
            return RegisterMessagesClasses(app, messageAssemblyMarkerTypes.Select((Type t) => t.GetTypeInfo().Assembly));
        }


        private static IApplicationBuilder RegisterMessagesClasses(IApplicationBuilder app, IEnumerable<Assembly> assembliesToScan)
        {
            var messageTypes = assembliesToScan
                .SelectMany(a => a.GetExportedTypes())
                .Where(type => !type.IsAbstract && typeof(IMorseMessage).IsAssignableFrom(type));

            foreach (var type in messageTypes)
            {
                var consumer = app.ApplicationServices.GetService(typeof(IMorseConsumer<>).MakeGenericType(type));
                consumer.GetType().GetMethod("Start")?.Invoke(consumer, new object[1] { new CancellationToken() });
            }
            return app;

        }
    }
}
