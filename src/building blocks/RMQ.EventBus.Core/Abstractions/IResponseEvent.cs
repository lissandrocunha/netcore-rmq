using RMQ.EventBus.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQ.EventBus.Core.Abstractions
{
    public interface IResponseEvent<TResponse> 
        where TResponse : IntegrationEvent
    {
    }
}
