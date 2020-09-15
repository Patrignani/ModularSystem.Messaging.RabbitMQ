using ModularSystem.Messaging.RabbitMQ.Core.Command;
using ModularSystem.Messaging.RabbitMQ.Core.DTOs;
using System.Threading.Tasks;


namespace ModularSystem.Messaging.RabbitMQ.Core.EventBus
{
    public interface IRabbitMQEventBus
    {
        Task<RabbitRPC<TResult>> PublishWaitAsync<TResult>(Command<TResult> command, string queueName = null);
        Task<TResult> PublishBasicRpcAsync<TResult>(Command<TResult> command, string queueName = null)
            where TResult : class;
        Task<bool> ExistQueue(string queueName);
        void PublishForget(Command.Command command, string queueName = null);
    }
}
