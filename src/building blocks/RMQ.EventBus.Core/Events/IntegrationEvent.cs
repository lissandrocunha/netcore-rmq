using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQ.EventBus.Core.Events
{
    public class IntegrationEvent
    {
        #region Variables

        private Guid _id;
        private DateTime _creationDate;

        #endregion

        #region Properties

        [JsonProperty]
        public Guid Id { get => _id; }
        [JsonProperty]
        public DateTime CreationDate { get => _creationDate; }

        #endregion

        #region Constructors

        public IntegrationEvent()
        {
            _id = Guid.NewGuid();
            _creationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            _id = id;
            _creationDate = createDate;
        }

        #endregion       

    }
}
