using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RMQ.EventBus.Core.Abstractions;
using RMQ.EventBus.Core.Abstractions.Objects;
using RMQ.EventBus.RabbitMQ.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQ.EventBus.RabbitMQ
{
    public class EventBusFactory
    {

        #region Variables

        private string _hostname;
        private int _port;
        private string _username;
        private string _password;
        private ICollection<IExchange> _exchanges;
        private ICollection<IQueue> _queues;
        private ICollection<IBinding> _bindings;
        private ConnectionFactory _connectionFactory;
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        public EventBusFactory(IReadOnlyDictionary<string, object> configurations,
                               ILogger logger)
        {
            _logger = logger;
            _hostname = configurations.TryGetValue("hostname", out object hostname) ? (string)hostname : default;
            _port = configurations.TryGetValue("port", out object port) ? (int)port : default;
            _username = configurations.TryGetValue("username", out object username) ? (string)username : default;
            _password = configurations.TryGetValue("password", out object password) ? (string)password : default;
            _exchanges = configurations.TryGetValue("exchanges", out object exchanges) ? (ICollection<IExchange>)exchanges : null;
            _queues = configurations.TryGetValue("queues", out object queues) ? (ICollection<IQueue>)queues : null;
            _bindings = configurations.TryGetValue("bindings", out object bindings) ? (ICollection<IBinding>)bindings : null;
        }

        #endregion

        #region Methods

        public ConnectionFactory CreateBusConnection()
        {
            _logger.LogInformation("Creating Bus...");
            if (string.IsNullOrWhiteSpace(_hostname))
            {
                _logger.LogError("Invalid ConnectionString({0}).", _hostname);
                return null;
            }
            _connectionFactory = new ConnectionFactory()
            {
                HostName = _hostname,
                Port = _port,
                UserName = _username,
                Password = _password
            };
            CreateBroker(_exchanges, _queues, _bindings);
            return _connectionFactory;
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
                    CreateExange(exchange as Exchange);
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
                    CreateQueue(queue as Queue);
                }
            }

            if (bindings != null && bindings.Count > 0)
            {
                foreach (var binding in bindings)
                {
                    CreateBinding(binding as Binding);
                }
            }

            _logger.LogInformation("Broker Created.");
        }

        private void CreateExange(Exchange exchange)
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

        private void CreateQueue(Queue queue)
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

        private void CreateBinding(Binding binding)
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

        #endregion

    }

}
