using ModularSystem.Messaging.RabbitMQ.Core.Command;
using ModularSystem.Messaging.RabbitMQ.Core.DTOs;
using System.Threading.Tasks;


namespace ModularSystem.Messaging.RabbitMQ.Core.EventBus
{
    public interface IRabbitMQEventBus
    {
        Task<RabbitRPC<TResult>> PublishWaitAsync<TResult>(Command<TResult> command, string queueName = null);

        Task PublishForgetAsync(Command.Command command);
        Task<bool> ExistQueue(string queueName);

        void PublishForget(Command.Command command);
    }
}
