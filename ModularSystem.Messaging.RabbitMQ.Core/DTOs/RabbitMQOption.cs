using System;

namespace ModularSystem.Messaging.RabbitMQ.Core.DTOs
{
    public class RabbitMQOption : RabbitMQTrackException
    {
        public RabbitMQOption()
        {
            Exception = (Exception e) =>
            {

            };
        }

        public string ConnectionFactory { get; private set; }

        public void AddConnection(string connection) => ConnectionFactory = connection;
        public void AddTrackException(Action<Exception> trackException) => Exception = trackException;

    }

    public class RabbitMQTrackException
    {
        public Action<Exception> Exception { get; protected set; }
    }
}
