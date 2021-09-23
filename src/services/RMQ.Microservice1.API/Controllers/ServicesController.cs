﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMQ.API.Core.Controllers;
using RMQ.EventBus.Core.Abstractions;
using RMQ.EventBus.Core.Abstractions.Objects;
using RMQ.EventBus.Core.Implementations.RabbitMQ.Objects;
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
            _bus.CreateBus("localhost", 5672, "guest", "guest",
                           new List<IExchange>() {
                               new ExchangeRMQ("eventbus","direct",true,false)
                           },
                           new List<IQueue>() {
                               new QueueRMQ("snoozing")
                           },
                           new List<IBinding>() {
                               new BindingRMQ(BindType.Queue, "eventbus",BindType.Queue, "snoozing","wakeup")
                           });
        }

        [HttpGet]
        [Route("status")]
        public ActionResult Status([FromQuery] string service)
        {
            object status;

            if (string.IsNullOrWhiteSpace(service))
            {
                status = _bus.Status();
            }
            else
            {
                status = _bus.PublishAsync(new StatusIntegrationEvent(service));
            }

            return CustomResponse(status);
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