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
        /// Show bus status
        /// </summary>
        /// <returns>bus informations</returns>
        object Status();

        /// <summary>
        /// Create a Bus
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="exchanges">Exchanges to create on Bus</param>
        /// <param name="queues">Queues to create on Bus</param>
        /// <param name="bindings">Bindings to create</param>
        public void CreateBus(string hostname,
                              int port,
                              string userName,
                              string password,
                              ICollection<IExchange> exchanges = null,
                              ICollection<IQueue> queues = null,
                              ICollection<IBinding> bindings = null);

        /// <summary>
        /// Publishes a event
        /// </summary>
        /// <param name="event">Event to Publish</param>
        /// <param name="exchange">Exange to use</param>
        /// <param name="routingKey">Tag to use</param>
        void Publish(IntegrationEvent @event, string exchange = "", string routingKey = "");


        public Task<IntegrationEvent> Consume<T>(string queue)
            where T : IntegrationEvent;


    }
}
