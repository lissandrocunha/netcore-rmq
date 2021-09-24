using Microsoft.Extensions.DependencyInjection;
using RMQ.EventBus.Core.Abstractions;
using RMQ.EventBus.Core.Abstractions.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMQ.Microservice1.API.Configuration
{
    public static class DIConfiguration
    {

        public static IServiceCollection AddDIConfiguration(this IServiceCollection services)
        {
            
            services.AddScoped<IEventBus, RMQ.EventBus.RabbitMQ.EventBus>();

            return services;
        }

    }
}
