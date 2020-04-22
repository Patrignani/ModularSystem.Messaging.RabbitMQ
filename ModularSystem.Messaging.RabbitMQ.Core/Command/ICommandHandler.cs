using System.Threading.Tasks;

namespace ModularSystem.Messaging.RabbitMQ.Core.Command
{
    public interface ICommandHandler<TCommand, TCommandResult>
    {
        Task<TCommandResult> HandleWithEventAsync(TCommand command);

    }

    public interface ICommandHandler<TCommand> where TCommand : Command
    {
        Task HandleWithEventAsync(TCommand command);

    }
}
