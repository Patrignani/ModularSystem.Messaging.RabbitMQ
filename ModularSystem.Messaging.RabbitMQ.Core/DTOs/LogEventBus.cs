using System;
using System.Collections.Generic;

namespace ModularSystem.Messaging.RabbitMQ.Core.DTOs
{
    public abstract class LogEventBus : Command.Command
    {
        public LogEventBus(
            Guid eventIdBus,
            DateTime eventCreateBus,
            string queueName,
            long busRunTimeMilliseconds,
            bool queueExist,
            bool sucess,
            List<string> messages)
        {
            EventIdBus = eventIdBus;
            EventCreateBus = eventCreateBus;
            QueueName = queueName;
            BusRunTimeMilliseconds = busRunTimeMilliseconds;
            QueueExist = queueExist;
            Sucess = sucess;
            Messages = String.Join(" ", messages);
        }

        public Guid EventIdBus { get; private set; }
        public DateTime EventCreateBus { get; private set; }
        public string QueueName { get; private set; }
        public long BusRunTimeMilliseconds { get; private set; }
        public bool QueueExist { get; private set; }
        public bool Sucess { get; private set; }
        public string Messages { get; private set; }
    }
}
