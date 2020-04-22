using ModularSystem.Messaging.RabbitMQ.Core.Command;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


namespace ModularSystem.Messaging.RabbitMQ.Extensions
{
    public static class SubscribeMessage
    {
        public static void SubscribeToCommand<TCommand>(this IModel channel,
      ICommandHandler<TCommand> handler) where TCommand : Command
        {
            var queueName = ConfigureServicesRabbitMQ.GetQueueName<TCommand>();

            channel.QueueDeclare(
                queue: ConfigureServicesRabbitMQ.GetQueueName<TCommand>(),
                durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) => {
                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                {
                    var message = Encoding.UTF8.GetString(body);
                    var requestCommand = JsonConvert.DeserializeObject<TCommand>(message);

                    var responseAwait = handler.HandleWithEventAsync(requestCommand);
                    responseAwait.Wait();
                }

            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        }
    }
}
