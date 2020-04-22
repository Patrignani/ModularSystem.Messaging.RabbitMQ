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
      ICommandHandler<TCommand, TCommandResult> handler,RabbitMQTrackException trackException) where TCommand : Command<TCommandResult>
        {
            var queueName = ConfigureServicesRabbitMQ.GetQueueName<TCommand>();
            channel.QueueDeclare(
                queue: ConfigureServicesRabbitMQ.GetQueueName<TCommand>(),
                durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(queueName, false, consumer);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                {
                    var message = Encoding.UTF8.GetString(body);
                    var requestCommand = JsonConvert.DeserializeObject<TCommand>(message);

                    var responseAwait = handler.HandleWithEventAsync(requestCommand).GetAwaiter();

                    responseAwait.OnCompleted(() =>
                    {
                        Exception exception = null;
                        try
                        {
                            var response = responseAwait.GetResult();
                            var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));

                            channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                basicProperties: replyProps, body: responseBytes);
                            channel.BasicAck(deliveryTag: ea.DeliveryTag,
                              multiple: false);
                        }
                        catch (Exception e)
                        {
                            exception = e;
                        }

                        if(exception != null)
                        {
                            channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                              basicProperties: replyProps, body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("")));
                            channel.BasicAck(deliveryTag: ea.DeliveryTag,
                              multiple: false);

                            trackException.Exception(exception);
                        }
                    });
                }
            };
        }
    }
}
