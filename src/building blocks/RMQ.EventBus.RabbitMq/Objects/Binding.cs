using RMQ.EventBus.Core.Abstractions.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQ.EventBus.RabbitMQ.Objects
{
    public class Binding : IBinding
    {
        private readonly BindType _sourceType;
        private readonly string _source;
        private readonly BindType _destinationType;
        private readonly string _destination;
        private readonly string _routeKey;
        private readonly IDictionary<string, object> _arguments;

        public BindType SourceType => _sourceType;
        public string Source => _source;
        public BindType DestinationType => _destinationType;
        public string Destination => _destination;
        public string RouteKey => _routeKey;
        public IDictionary<string, object> Arguments => _arguments;

        public Binding(BindType sourceType,
                          string source,
                          BindType destinationType,
                          string destination,
                          string routeKey = "",
                          IDictionary<string, object> arguments = null)
        {
            _sourceType = sourceType;
            _source = source;
            _destinationType = destinationType;
            _destination = destination;
            _routeKey = routeKey;
            _arguments = arguments;
        }


    }

    public enum BindType
    {
        Exchange,
        ExchangeBindNoWait,
        Queue,
        QueueBindNoWait
    }
}
