using RMQ.EventBus.Core.Events;
using System.Threading.Tasks;

namespace RMQ.EventBus.Core.Abstractions
{

    public interface IIntegrationEventHandler
    {
    }

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
    
}