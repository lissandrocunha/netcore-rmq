using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RMQ.EventBus.Core.Abstractions;
using RMQ.EventBus.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQ.EventBus.RabbitMQ
{
    public class EventBus : IEventBus
    {

        #region Variables

        private ConnectionFactory _connectionFactory;
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        public EventBus(ILogger<EventBus> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        public void CreateBus(IReadOnlyDictionary<string, object> configurations)
        {
            _connectionFactory = new EventBusFactory(configurations, _logger).CreateBusConnection();
        }

        public void Publish(IntegrationEvent @event, string exchange = "", string routingKey = "")
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
                    channel.ConfirmSelect();
                    channel.BasicAcks += Evento_Confirmacao;
                    channel.BasicNacks += Evento_NaoConfirmacao;

                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
                    routingKey = string.IsNullOrWhiteSpace(routingKey) ? @event.GetType().Name?.ToLower().Replace("integrationevent", "") : routingKey;

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: exchange,
                                         routingKey: routingKey,
                                         basicProperties: properties,
                                         body: body);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on Publishing: {0}", ex);
                return;
            }
            _logger.LogInformation("Event Published.");
        }

        public Task<IntegrationEvent> Consume<T>(string queue) where T : IntegrationEvent
        {
            IntegrationEvent message = null;
            _logger.LogInformation("Consuming Messages on Queue {0} on RabbitMQ...", queue);

            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ConfirmSelect();
                    channel.BasicAcks += Evento_Confirmacao;
                    channel.BasicNacks += Evento_NaoConfirmacao;

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                    };

                    channel.BasicConsume(queue,
                                         true,
                                         consumer);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on Consuming Messages on Queue {0} on RabbitMQ...", queue);
                return null;
            }

            _logger.LogInformation("Message Consumed.");
            return Task.FromResult(message);
        }

        public Task<TResponse> Request<TRequest, TResponse>(TRequest @event, string exchange = "", string routingKey = "", string replyQueue = "")
            where TRequest : IntegrationEvent
            where TResponse : IResponseEvent<TRequest>
        {

            _logger.LogInformation("Request Event({0} - ID:{1}, Created: {2}) on RabbitMQ...",
                                 @event.GetType().Name,
                                 @event.Id,
                                 @event.CreationDate);
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ConfirmSelect();
                    channel.BasicAcks += Evento_Confirmacao;
                    channel.BasicNacks += Evento_NaoConfirmacao;



                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
                    routingKey = string.IsNullOrWhiteSpace(routingKey) ? @event.GetType().Name?.ToLower().Replace("integrationevent", "") : routingKey;

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.ReplyTo = replyQueue;

                    channel.BasicPublish(exchange: exchange,
                                         routingKey: routingKey,
                                         basicProperties: properties,
                                         body: body);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on Publishing: {0}", ex);
                return null;
            }
            _logger.LogInformation("Event Published.");

            return null;
        }

        public Task<TResponse> Response<TRequest, TResponse>(string replyQueue, Action<TResponse> @response)
            where TRequest : IntegrationEvent
            where TResponse : IResponseEvent<TRequest>
        {

            _logger.LogInformation("Response Event({0} on RabbitMQ...",
                                   @response.GetType().Name);
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ConfirmSelect();
                    channel.BasicAcks += Evento_Confirmacao;
                    channel.BasicNacks += Evento_NaoConfirmacao;

                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@response));

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.ReplyTo = replyQueue;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on Publishing: {0}", ex);
                return null;
            }
            _logger.LogInformation("Event Published.");

            return null;
        }

        public IDisposable RespondAsync<TRequest, TResponse>(string replyQueue, Func<TRequest, Task<TResponse>> responder)
            where TRequest : IntegrationEvent where TResponse : ResponseMessage
        {
            throw new NotImplementedException();
        }

        private void Evento_Confirmacao(object sender, BasicAckEventArgs e)
        {
            _logger.LogInformation("Confirmação do evento: {0} - {1}", sender.ToString(), e.DeliveryTag);
        }

        private void Evento_NaoConfirmacao(object sender, BasicNackEventArgs e)
        {
            _logger.LogError("Erro de confirmação do evento: {0} - {1}", sender.ToString(), e.DeliveryTag);
        }

        public void Dispose()
        {
            if (_connectionFactory != null)
            {

            }
        }

        #endregion
    }
}
