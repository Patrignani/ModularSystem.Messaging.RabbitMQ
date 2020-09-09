using ModularSystem.Messaging.RabbitMQ.Core.Command;
using ModularSystem.Messaging.RabbitMQ.Core.DTOs;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ModularSystem.Messaging.RabbitMQ.Extensions
{
    public static class SubscribeMessage
    {
        public static void SubscribeToCommand<TCommand>(this IModel channel,
      ICommandHandler<TCommand> handler, QueueConfigurationOptionsSubscribe option) where TCommand : Command
        {
            var queueName = String.IsNullOrEmpty(option.QueueName) ? ConfigureServicesRabbitMQ.GetQueueName<TCommand>() : option.QueueName;

            channel.QueueDeclare(
                queue: queueName,
                durable: option.Durable,
                exclusive: option.Exclusive,
                autoDelete: option.AutoDelete,
                arguments: option.Arguments);

            if (!string.IsNullOrEmpty(option.ExchangeName) || !string.IsNullOrEmpty(option.RoutingKey))
            {
                channel.QueueBind(
                    queue: queueName,
                    exchange: option.ExchangeName,
                    routingKey: option.RoutingKey);
            }

            if (option.BasicQos)
                channel.BasicQos(0, 1, false);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                {
                    var message = Encoding.UTF8.GetString(body);
                    var requestCommand = JsonConvert.DeserializeObject<TCommand>(message);

                    await handler.HandleWithEventAsync(requestCommand);
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        }
    }
}
