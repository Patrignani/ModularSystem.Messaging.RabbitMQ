using ModularSystem.Messaging.RabbitMQ.Core.Enum;
using System.Collections.Generic;

namespace ModularSystem.Messaging.RabbitMQ.Core.DTOs
{
    public class QueueConfigurationOptionsSubscribe 
    {
        public IocTypes Types { get; set; } = IocTypes.Scoped;
        public string QueueName { get; set; } = "";
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = false;
        public string ExchangeName { get; set; } = "";
        public string RoutingKey { get; set; } = "";
        public bool AutoAck { get; set; } = true;
        public bool BasicQos { get; set; } = false;
        public IDictionary<string, object> Arguments { get; set; } = null;
    }

}