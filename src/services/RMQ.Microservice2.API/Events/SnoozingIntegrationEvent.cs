using RMQ.EventBus.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMQ.Microservice2.API.Events
{
    public class SnoozingIntegrationEvent : IntegrationEvent
    {
        private readonly string _queue;

        public string Queue => _queue;

        public SnoozingIntegrationEvent(string queue)
        {
            _queue = queue;
        }

        
    }
}
