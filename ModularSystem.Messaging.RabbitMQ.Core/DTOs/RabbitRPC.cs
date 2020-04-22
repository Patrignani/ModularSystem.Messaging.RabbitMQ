using System;
using System.Collections.Generic;
using System.Text;

namespace ModularSystem.Messaging.RabbitMQ.Core.DTOs
{
    public class RabbitRPC<TResult>
    {
        public RabbitRPC()
        {
            MessageTask = new MessageTask();
        }

        public MessageTask MessageTask { get; set; }
        public TResult Data { get; set; }
    }
}
