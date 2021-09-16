using System.Threading.Tasks;

namespace RMQ.EventBus.Core.Interfaces
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}