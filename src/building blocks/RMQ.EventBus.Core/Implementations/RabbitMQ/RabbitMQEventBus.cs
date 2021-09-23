using Microsoft.Extensions.Logging;
using RMQ.EventBus.Core.Events;
using RMQ.EventBus.Core.Abstractions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RMQ.EventBus.Core.Abstractions.Objects;
using RMQ.EventBus.Core.Implementations.RabbitMQ.Objects;

namespace RMQ.EventBus.Core.Implementations.RabbitMQ
{
    public class RabbitMQEventBus : IEventBus
    {

        #region Variables

        private string _hostname;
        private int _port;
        private string _userName;
        private string _password;
        private ConnectionFactory _connectionFactory;
        private ICollection<IExchange> _exchanges;
        private ICollection<IQueue> _queues;
        private ICollection<IBinding> _bindings;
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        public RabbitMQEventBus(ILogger<RabbitMQEventBus> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        public object Status()
        {
            object status;
            _logger.LogInformation("Getting Status...");

            //if (_bus == null)
            //{
            //    status = new
            //    {
            //        status = "Bus not created.",
            //        connectionString = _connectionString
            //    };

            //}
            //else
            //{
            //    status = new
            //    {
            //        status = "OK",
            //        connectionString = _connectionString
            //    };
            //}

            //_logger.LogInformation(status.ToString());

            return null;// status;
        }

        public void CreateBus(string hostname,
                              int port,
                              string userName,
                              string password,
                              ICollection<IExchange> exchanges,
                              ICollection<IQueue> queues,
                              ICollection<IBinding> bindings)
        {
            _logger.LogInformation("Creating Bus...");
            _hostname = hostname;
            _port = port;
            _userName = userName;
            _password = password;
            _exchanges = exchanges;
            _queues = queues;
            _bindings = bindings;
            if (string.IsNullOrWhiteSpace(_hostname))
            {
                _logger.LogError("Invalid ConnectionString({0}).", hostname);
                return;
            }
            _connectionFactory = new ConnectionFactory()
            {
                HostName = _hostname,
                Port = _port,
                UserName = _userName,
                Password = _password
            };
            CreateBroker(_exchanges, _queues, _bindings);
        }

        private void CreateBroker(ICollection<IExchange> exchanges,
                                  ICollection<IQueue> queues,
                                  ICollection<IBinding> bindings)
        {
            _logger.LogInformation("Creating Broker...");

            if (exchanges == null || exchanges.Count <= 0)
            {
                _logger.LogInformation("No Exchanges found.");
            }
            else
            {
                foreach (var exchange in exchanges)
                {
                    CreateExange(exchange as ExchangeRMQ);
                }
            }

            if (queues == null || queues.Count <= 0)
            {
                _logger.LogInformation("No Queues found.");
            }
            else
            {
                foreach (var queue in queues)
                {
                    CreateQueue(queue as QueueRMQ);
                }
            }

            if (bindings != null && bindings.Count > 0)
            {
                foreach (var binding in bindings)
                {
                    CreateBinding(binding as BindingRMQ);
                }
            }

            _logger.LogInformation("Broker Created.");
        }

        private void CreateExange(ExchangeRMQ exchange)
        {

            _logger.LogInformation("Creating Exchange {0}...", exchange.Name);
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange.Name,
                                            exchange.Type,
                                            exchange.Durable,
                                            exchange.AutoDelete);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exchange.");
                return;
            }

            _logger.LogInformation("Exchange {0} created.", exchange);
        }

        private void CreateQueue(QueueRMQ queue)
        {
            _logger.LogInformation("Creating Queue {0}...", queue.Name);
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue.Name,
                                         queue.Durable,
                                         queue.Exclusive,
                                         queue.AutoDelete);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating queue.");
                return;
            }

            _logger.LogInformation("Queue {0} created.", queue.Name);
        }

        private void CreateBinding(BindingRMQ binding)
        {
            _logger.LogInformation("Creating Binding({0}): {1} to {2}...", binding.SourceType, binding.Source, binding.Destination);
            try
            {

                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {

                    switch (binding.SourceType)
                    {
                        case BindType.Exchange:

                            channel.ExchangeBind(binding.Destination,
                                                 binding.Source,
                                                 binding.RouteKey,
                                                 binding.Arguments);

                            break;
                        case BindType.ExchangeBindNoWait:

                            channel.ExchangeBindNoWait(binding.Destination,
                                                       binding.Source,
                                                       binding.RouteKey,
                                                       binding.Arguments);
                            break;
                        case BindType.Queue:

                            channel.QueueBind(binding.Destination,
                                              binding.Source,
                                              binding.RouteKey,
                                              binding.Arguments);
                            break;
                        case BindType.QueueBindNoWait:

                            channel.QueueBindNoWait(binding.Source,
                                                    binding.Destination,
                                                    binding.RouteKey,
                                                    binding.Arguments);
                            break;
                    }


                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating binding.");
                return;
            }

        }


        public void Publish(IntegrationEvent @event, string exchange, string routingKey)
        {
            _logger.LogInformation("Publishing Event({0} - ID:{1}, Created: {2}) on RabbitMQ...",
                                   @event.GetType().Name,
                                   @event.Id,
                                   @event.CreationDate);
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
                    routingKey = string.IsNullOrWhiteSpace(routingKey) ? @event.GetType().Name?.ToLower().Replace("integrationevent", "") : routingKey;

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: exchange,
                                         routingKey: routingKey,
                                         basicProperties: properties,
                                         body: body);

                    //channel.BasicPublish(exchange: exchange,
                    //                     routingKey: @event.GetType().Name.Replace("IntegrationEvent", "")?.ToLower(),
                    //                     basicProperties: null,
                    //                     body: body);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on Publishing: {0}", ex);
                return;
            }
            _logger.LogInformation("Event Published.");
        }

        public Task PublishAsync(IntegrationEvent @event)
        {
            _logger.LogInformation("Publishing Event({0} - ID:{1}, Created: {2}) on RabbitMQ...",
                                   @event.GetType().Name,
                                   @event.Id,
                                   @event.CreationDate);

            try
            {
                //var success = _bus.PubSub.PublishAsync(@event)
                //                         .ContinueWith(task =>
                //                         {
                //                             switch (task.Status)
                //                             {
                //                                 case TaskStatus.Created:
                //                                     _logger.LogInformation("Event Created...");
                //                                     break;
                //                                 case TaskStatus.WaitingForActivation:
                //                                     _logger.LogInformation("Event Waiting for Activation...");
                //                                     break;
                //                                 case TaskStatus.WaitingToRun:
                //                                     _logger.LogInformation("Event Waiting to Run...");
                //                                     break;
                //                                 case TaskStatus.Running:
                //                                     _logger.LogInformation("Event Running...");
                //                                     break;
                //                                 case TaskStatus.WaitingForChildrenToComplete:
                //                                     _logger.LogInformation("Event Waiting Children(s) to Complete...");
                //                                     break;
                //                                 case TaskStatus.RanToCompletion:
                //                                     _logger.LogInformation("Event Ran to Completion...");
                //                                     break;
                //                                 case TaskStatus.Canceled:
                //                                     _logger.LogInformation("Event Canceled...");
                //                                     break;
                //                                 case TaskStatus.Faulted:
                //                                     _logger.LogError("Event Faulted.");
                //                                     break;
                //                             }
                //                         });

                _logger.LogInformation("Event Published.");
                return null;// success;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on Publishing: {0}", ex);
                return Task.CompletedTask;
            }
        }

        public void Respond<TRequest, TResponse>()
            where TRequest : IntegrationEvent
            where TResponse : IIntegrationEventHandler
        {

        }

        public void Subscribe<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler
        {

        }

        public void Unsubscribe<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler
        {
            throw new NotImplementedException();
        }

        //public TR Request<T, TR>(T @event)
        //    where T : IntegrationEvent
        //    where TR : IIntegrationEventHandler<T>
        //{
        //    _logger.LogInformation("Request Event({0} - ID:{1}, Created: {2}) on RabbitMQ...",
        //                           @event.GetType().Name,
        //                           @event.Id,
        //                           @event.CreationDate);

        //    //try
        //    //{
        //    //var success = _bus.Rpc.Request<T, TR>(@event);

        //    _logger.LogInformation("Event Published.");
        //    return success;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    _logger.LogError("Error on Publishing: {0}", ex);
        //    //    return Task.FromResult(null);
        //    //}
        //}

        //public async Task<TR> RequestAsync<T, TR>(T @event)
        //    where T : IntegrationEvent
        //    where TR : IIntegrationEventHandler<T>
        //{
        //    _logger.LogInformation("Request Event({0} - ID:{1}, Created: {2}) on RabbitMQ...",
        //                           @event.GetType().Name,
        //                           @event.Id,
        //                           @event.CreationDate);

        //    //try
        //    //{
        //    //var success = await _bus.Rpc.RequestAsync<T, TR>(@event);

        //    _logger.LogInformation("Event Published.");
        //    return TR;
        //    //return success;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    _logger.LogError("Error on Publishing: {0}", ex);
        //    //    return null;
        //    //}
        //}

        public void Dispose()
        {
            //if (_bus != null)
            //{
            //    _bus.Dispose();
            //}
        }

        #endregion

    }
}
