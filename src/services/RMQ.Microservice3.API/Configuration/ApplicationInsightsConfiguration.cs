using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMQ.Microservice3.API.Configuration
{
    public static class ApplicationInsightsConfiguration
    {

        public static IServiceCollection AddApplicationInsightsConfiguration(this IServiceCollection services,
                                                                             IConfiguration configuration)
        {

            services.AddApplicationInsightsTelemetry(configuration);

            return services;
        }

        public static IApplicationBuilder UseApplicationInsightsConfiguration(this IApplicationBuilder app,
                                                                              IWebHostEnvironment env)
        {

            return app;
        }

    }
}
