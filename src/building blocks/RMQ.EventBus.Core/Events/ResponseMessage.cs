using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQ.EventBus.Core.Events
{
    public class ResponseMessage : IntegrationEvent
    {
        private object v;

        public ResponseMessage(object v)
        {
            this.v = v;
        }
    }
}
