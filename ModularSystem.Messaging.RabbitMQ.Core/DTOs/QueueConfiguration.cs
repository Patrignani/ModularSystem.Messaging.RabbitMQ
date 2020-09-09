using ModularSystem.Messaging.RabbitMQ.Core.Enum;
using System.Collections.Generic;

namespace ModularSystem.Messaging.RabbitMQ.Core.DTOs
{
    public class QueueConfigurationOptionsReply
    {
        public IocTypes Types { get; set; } = IocTypes.Scoped;
        public string QueueName { get; set; } = "";
        public bool Durable { get; set; } = false;
        public bool Exclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = false;
        public IDictionary<string, object> Arguments { get; set; } = null;
    }
}