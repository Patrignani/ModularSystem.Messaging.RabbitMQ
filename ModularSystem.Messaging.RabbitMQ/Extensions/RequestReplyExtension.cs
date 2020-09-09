using ModularSystem.Messaging.RabbitMQ.Core.Command;
using ModularSystem.Messaging.RabbitMQ.Core.DTOs;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ModularSystem.Messaging.RabbitMQ.Extensions
{
    public static class RequestReplyExtension
    {
        public static void WithRequestReply<TCommand, TCommandResult>(this IModel channel,
        ICommandHandler<TCommand, TCommandResult> handler, RabbitMQTrackException trackException,
        QueueConfigurationOptionsReply option)
            where TCommand : Command<TCommandResult>
        {
            var queueName = String.IsNullOrEmpty(option.QueueName) ? ConfigureServicesRabbitMQ.GetQueueName<TCommand>() : option.QueueName;
            channel.QueueDeclare(
                queue: queueName,
                durable: option.Durable,
            exclusive: option.Exclusive,
            autoDelete: option.AutoDelete,
            arguments: option.Arguments);

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(queueName, false, consumer);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                var message = Encoding.UTF8.GetString(body.ToArray());
                var requestCommand = JsonConvert.DeserializeObject<TCommand>(message);

                var responseAwait = handler.HandleWithEventAsync(requestCommand).GetAwaiter();
                byte[] responseBytes = null;

                responseAwait.OnCompleted(() =>
                {
                    try
                    {
                        var response = responseAwait.GetResult();
                        responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));

                    }
                    catch (Exception e)
                    {
                        responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(""));
                        trackException.Exception(e);
                    }
                    finally
                    {
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                             basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag,
                          multiple: false);
                    }

                });

            };
        }
    }
}
