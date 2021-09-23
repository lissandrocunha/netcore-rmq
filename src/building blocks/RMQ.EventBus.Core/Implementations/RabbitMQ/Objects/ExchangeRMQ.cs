using RMQ.EventBus.Core.Abstractions.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQ.EventBus.Core.Implementations.RabbitMQ.Objects
{
    public class ExchangeRMQ : IExchange
    {
        private readonly string _name;
        private readonly string _type;
        private readonly bool _durable = false;
        private readonly bool _autoDelete = false;

        public string Name { get => _name; }
        public string Type { get => _type; }
        public bool Durable { get => _durable; }
        public bool AutoDelete { get => _autoDelete; }

        public ExchangeRMQ(string name,
                           string type,
                           bool durable = false,
                           bool autoDelete = false)
        {
            _name = name;
            _type = type;
            _durable = durable;
            _autoDelete = autoDelete;
        }

    }
}
