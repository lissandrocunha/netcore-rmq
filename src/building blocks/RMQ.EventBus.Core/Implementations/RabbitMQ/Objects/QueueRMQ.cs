using RMQ.EventBus.Core.Abstractions.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQ.EventBus.Core.Implementations.RabbitMQ.Objects
{
    public class QueueRMQ : IQueue
    {
        private readonly string _name;
        private readonly bool _durable = true;
        private readonly bool _exclusive = false;
        private readonly bool _autoDelete = false;

        public string Name { get => _name; }
        public bool Durable { get => _durable; }
        public bool Exclusive { get => _exclusive; }
        public bool AutoDelete { get => _autoDelete; }

        public QueueRMQ(string name,
                        bool durable = true,
                        bool exclusive = false,
                        bool autoDelete = false)
        {
            _name = name;
            _durable = durable;
            _exclusive = exclusive;
            _autoDelete = autoDelete;
        }


    }
}
