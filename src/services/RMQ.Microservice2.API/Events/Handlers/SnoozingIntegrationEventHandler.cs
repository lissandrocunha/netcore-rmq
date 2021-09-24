using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RMQ.EventBus.Core.Abstractions;
using RMQ.EventBus.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RMQ.Microservice2.API.Events.Handlers
{
    public class SnoozingIntegrationEventHandler : BackgroundService
    {


        #region Variables

        private IEventBus _bus;
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        public SnoozingIntegrationEventHandler(IEventBus bus,
                                               ILogger<SnoozingIntegrationEventHandler> logger)
        {
            _logger = logger;
            _bus = bus;
            _bus.CreateBus(new Dictionary<string, object>() {
                {"hostname","localhost" },
                {"port",5672 },
                {"username","guest" },
                {"password","guest" }
            });
            
        }

        #endregion

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //_bus.RespondAsync<SnoozingIntegrationEvent, ResponseMessage>(async request => new ResponseMessage(await ));


            return Task.CompletedTask;
        }
    }
}
