using RMQ.EventBus.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMQ.Microservice1.API.Events
{
    public class StatusIntegrationEvent : IntegrationEvent
    {

        private readonly string _service;

        public string Service => _service;

        public StatusIntegrationEvent(string service)
        {
            _service = service;
        }


    }
}
