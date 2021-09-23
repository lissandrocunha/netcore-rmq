using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMQ.API.Core.Controllers;
using RMQ.EventBus.Core.Abstractions;
using RMQ.Microservice2.API.Events;
using RMQ.Microservice2.API.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMQ.Microservice2.API.Controllers
{

    [ApiController]
    public class ServicesController : BaseController
    {

        private IEventBus _bus;

        public ServicesController(IEventBus bus)
        {
            _bus = bus;
            //_bus.CreateBus("localhost", 5672, "guest", "guest",
            //               new Dictionary<string, string>() { { "eventbus", "direct" } });
        }

        [HttpGet]
        [Route("snoozing")]
        public ActionResult Snoozing()
        {
            var snoozingEvent = new SnoozingIntegrationEvent("service2");

            //_bus.Subscribe<SnoozingIntegrationEvent, SnoozingIntegrationEventHandler>();

            return CustomResponse("Teste OK!");
        }

    }
}
