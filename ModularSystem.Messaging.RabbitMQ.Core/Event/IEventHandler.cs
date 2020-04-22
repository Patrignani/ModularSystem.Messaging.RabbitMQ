using System.Threading.Tasks;

namespace ModularSystem.Messaging.RabbitMQ.Core.Event
{
    public interface IEventHandler<in TEvent> where TEvent : Event
    {
        Task HandleAsync(TEvent @event);
    }

    public interface IDynamicEventHandler
    {
        Task HandleAsync(dynamic @event);
    }
}
