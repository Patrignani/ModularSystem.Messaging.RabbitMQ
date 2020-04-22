using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace ModularSystem.Messaging.RabbitMQ.Core.DTOs
{
    public class MessageTask : Event.Event
    {
        public MessageTask()
        {
            EventIdBus = Guid.NewGuid();
            EventCreateBus = DateTime.UtcNow;
            Messages = new List<string>();
            Stopwatch = new Stopwatch();
        }

        public Guid EventIdBus { get; private set; }
        public DateTime EventCreateBus { get; private set; }
        public string QueueName { get; private set; }
        public long BusRunTimeMilliseconds { get; private set; }
        public bool QueueExist { get; private set; }
        public List<string> Messages { get; private set; }
        private Stopwatch Stopwatch { get; set; }

        public void ExistQueue() => QueueExist = true;
        public void NotExistQueue() => QueueExist = false;

        public void SetMessage(string message) => Messages.Add(message);
        public void SetMessages(List<string> messages) => Messages.AddRange(messages);
        public void SetQueueName(string name) => QueueName = name;

        public void StartCounting() => Stopwatch.Start();

        public void StopCounting()
        {
            Stopwatch.Stop();
            BusRunTimeMilliseconds = Stopwatch.ElapsedMilliseconds;
        }

    }
}
