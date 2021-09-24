using RMQ.EventBus.Core.Abstractions.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQ.EventBus.RabbitMQ.Objects
{
    public class Exchange : IExchange
    {
        private readonly string _name;
        private readonly string _type;
        private readonly bool _durable = false;
        private readonly bool _autoDelete = false;
        private readonly IDictionary<string, object> _arguments;

        public string Name => _name;
        public string Type => _type;
        public bool Durable => _durable;
        public bool AutoDelete => _autoDelete;
        public IDictionary<string, object> Arguments => _arguments;

        public Exchange(string name,
                        string type,
                        bool durable = false,
                        bool autoDelete = false,
                        IDictionary<string, object> arguments = null)
        {
            _name = name;
            _type = type;
            _durable = durable;
            _autoDelete = autoDelete;
            _arguments = arguments;
        }

    }
}
