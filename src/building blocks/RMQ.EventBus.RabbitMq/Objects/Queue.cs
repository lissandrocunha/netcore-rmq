using RMQ.EventBus.Core.Abstractions.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQ.EventBus.RabbitMQ.Objects
{
    public class Queue : IQueue
    {
        private readonly string _name;
        private readonly bool _durable = true;
        private readonly bool _exclusive = false;
        private readonly bool _autoDelete = false;
        private readonly IDictionary<string, object> _arguments;

        public string Name => _name;
        public bool Durable => _durable;
        public bool Exclusive => _exclusive;
        public bool AutoDelete => _autoDelete;
        public IDictionary<string, object> Arguments => _arguments;

        public Queue(string name,
                        bool durable = true,
                        bool exclusive = false,
                        bool autoDelete = false,
                        IDictionary<string, object> arguments = null)
        {
            _name = name;
            _durable = durable;
            _exclusive = exclusive;
            _autoDelete = autoDelete;
            _arguments = arguments;
        }


    }
}
