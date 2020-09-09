using ModularSystem.Messaging.RabbitMQ.Core.Command;
using ModularSystem.Messaging.RabbitMQ.Core.DTOs;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;

namespace ModularSystem.Messaging.RabbitMQ.Extensions
{
    public class PublishMessage
    {
        private readonly RabbitMQTrackException _traceException;
        public PublishMessage(RabbitMQTrackException traceException)
        {
            _traceException = traceException;
        }

        public void Publish<ICommand>(IModel channel, ICommand command)
        {
            var queuName = command.GetType().Name;
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.QueueDeclare(queuName, durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command));

            channel.BasicPublish(exchange: "",
                                 routingKey: queuName,
                                 basicProperties: properties,
                                 body: body);
        }

        public async Task<TResult> SendAsync<TResult>(IModel channel, Command<TResult> command, string queueName = null, bool deleteQueue = true)
        {
            var replyQueueName = channel.QueueDeclare().QueueName;
            var respQueue = new BlockingCollection<string>();

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command));
            var properties = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            properties.CorrelationId = correlationId;
            properties.ReplyTo = replyQueueName;
            var replyToDelete = properties.ReplyTo;

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += Consumer_Received;
            void Consumer_Received(object sender, BasicDeliverEventArgs e)
            {
                var body = e.Body;
                var response = Encoding.UTF8.GetString(body.ToArray());
                if (e.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(response);

                    if(deleteQueue)
                    channel.QueueDelete(replyToDelete);
                }
            }

            var routingKey = string.IsNullOrEmpty(queueName) ? command.GetType().Name : queueName;

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: routingKey,
                basicProperties: properties,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);


            var value = JsonConvert.DeserializeObject<TResult>(respQueue.Take());

            return await Task.FromResult(value);
        }
    }
}
