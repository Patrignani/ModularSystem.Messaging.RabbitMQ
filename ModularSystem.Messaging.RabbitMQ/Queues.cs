using ModularSystem.Messaging.RabbitMQ.Core.Command;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularSystem.Messaging.RabbitMQ
{
   public class Queues
    {
        private IModel _bus;

        public Queues(IModel bus)
        {
            _bus = bus;
        }

        public Queues DeleteQueue<TCommand>() where TCommand : ICommand
        {
            var queueName = ConfigureServicesRabbitMQ.GetQueueName<TCommand>();
            _bus.QueueDelete(queueName);
            return this;
        }
    }
}
