using RMQ.EventBus.Core.Abstractions.Objects;
using RMQ.EventBus.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQ.EventBus.Core.Abstractions
{
    public interface IEventBus : IDisposable
    {


        /// <summary>
        /// Create a Bus
        /// </summary>
        /// <param name="configurations">Dictionary with params to create a bus</param>
        public void CreateBus(IReadOnlyDictionary<string, object> configurations);

        /// <summary>
        /// Publishes a event
        /// </summary>
        /// <param name="event">Event to Publish</param>
        /// <param name="exchange">Exange to use</param>
        /// <param name="routingKey">Tag to use</param>
        void Publish(IntegrationEvent @event, string exchange = "", string routingKey = "");

        /// <summary>
        /// Consume a event in a queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <returns></returns>
        public Task<IntegrationEvent> Consume<T>(string queue)
            where T : IntegrationEvent;


        IDisposable RespondAsync<TRequest, TResponse>(string replyQueue,
                                                      Func<TRequest, Task<TResponse>> responder)
            where TRequest : IntegrationEvent
            where TResponse : ResponseMessage;
    }
}
