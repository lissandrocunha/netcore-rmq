using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMQ.API.Core.Controllers;
using RMQ.EventBus.Core.Abstractions;
using RMQ.EventBus.Core.Abstractions.Objects;
using RMQ.EventBus.RabbitMQ.Objects;
using RMQ.Microservice1.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RMQ.Microservice1.API.Controllers
{
    [ApiController]
    public class ServicesController : BaseController
    {

        private IEventBus _bus;

        public ServicesController(IEventBus bus)
        {
            _bus = bus;

            _bus.CreateBus(new Dictionary<string, object>() {
                {"hostname","localhost" },
                {"port",5672 },
                {"username","guest" },
                {"password","guest" },
                {"exchanges", new List<IExchange>() {
                               new Exchange("eventbus","direct",true,false),
                               new Exchange("eventbus-dlx","direct",true,false) }},
                {"queues",  new List<IQueue>() {
                               new Queue("snoozing",arguments: new Dictionary<string,object>{ }),
                               new Queue("snoozing-dlx")}},
                {"bindings",new List<IBinding>() {
                               new Binding(BindType.Queue, "eventbus",BindType.Queue, "snoozing","wakeup")} }
            });

        }

        [HttpGet]
        [Route("wakeup")]
        public ActionResult WakeUp([FromQuery] string service)
        {
            var wuEvent = new WakeUpIntegrationEvent(service);

            _bus.Publish(wuEvent, "eventbus");

            return CustomResponse("Teste OK!");
        }

    }
}
