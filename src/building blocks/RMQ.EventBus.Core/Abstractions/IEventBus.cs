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
        /// 
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="exchanges"></param>
        /// <param name="queues"></param>
        /// <param name="bindings"></param>
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
        void Publish(IntegrationEvent @event, string exchange = "", string routingKey = "");

        /// <summary>
        /// Publishes event assinchronous
        /// </summary>
        /// <param name="event"></param>
        Task PublishAsync(IntegrationEvent @event);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        //TEventHandler Request<TEvent, TEventHandler>(TEvent @event)
        //    where TEvent : IntegrationEvent
        //    where TEventHandler : IIntegrationEventHandler<TEvent>;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        //Task<TEventHandler> RequestAsync<TEvent, TEventHandler>(TEvent @event)
        //    where TEvent : IntegrationEvent
        //    where TEventHandler : IIntegrationEventHandler<TEvent>;

        //void Subscribe<TEvent, TEventHandler>()
        //    where TEvent : IntegrationEvent
        //    where TEventHandler : IIntegrationEventHandler;

        //void Unsubscribe<TEvent, TEventHandler>()
        //    where TEvent : IntegrationEvent
        //    where TEventHandler : IIntegrationEventHandler;

    }
}
